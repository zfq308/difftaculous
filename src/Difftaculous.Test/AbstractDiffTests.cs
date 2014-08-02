﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Difftaculous.Caveats;
using Difftaculous.Paths;
using Difftaculous.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Shouldly;


namespace Difftaculous.Test
{
    [TestFixture]
    public abstract class AbstractDiffTests
    {
        protected abstract IDiffResult DoCompare(object a, object b, IEnumerable<ICaveat> caveats = null);


        public class SimpleObject
        {
            public string Name { get; set; }
        }


        [Test]
        public void SimpleObjectComparedWithItselfHasNoDifferences()
        {
            SimpleObject a = new SimpleObject { Name = "Value" };
            SimpleObject b = a;

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(true);
        }



        [Test]
        public void NullValuesAreTolerated()
        {
            SimpleObject a = new SimpleObject { Name = null };
            SimpleObject b = a;

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(true);
        }



        [Test]
        public void AlteredValueResultsInOneDifference()
        {
            SimpleObject a = new SimpleObject { Name = "One" };
            SimpleObject b = new SimpleObject { Name = "Two" };

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(false);
            result.Annotations.ShouldContain(x => x.Path.Equals(DiffPath.FromJsonPath("$.name")));
        }



        [Test]
        public void SimpleArrayComparedWithItselfHasNoDifferences()
        {
            string[] a = { "Red", "Green", "Blue" };
            string[] b = new string[3];
            Array.Copy(a, b, 3);

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(true);
        }



        [Test]
        public void AlteredSimpleArrayResultsInOneDifference()
        {
            string[] a = { "Red", "Green", "Blue" };
            string[] b = { "Red", "Black", "Blue" };

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(false);

            // TODO - verify annotation!
            result.Annotations.ShouldContain(x => x.Path.Equals(DiffPath.FromJsonPath("$[1]")));
        }





        public class NestedObject
        {
            public SimpleObject Thing { get; set; }
        }



        [Test]
        public void AlteredNestedPropertyResultsInOneDifference()
        {
            var a = new NestedObject { Thing = new SimpleObject { Name = "Fred" } };
            var b = new NestedObject { Thing = new SimpleObject { Name = "Barney" } };

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(false);
            result.Annotations.ShouldContain(x => x.Path.Equals(DiffPath.FromJsonPath("$.thing.name")));
        }




        public class TwoProp
        {
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
        }


        public class TwoPropRev
        {
            public string Prop2 { get; set; }
            public string Prop1 { get; set; }
        }



        [Test]
        public void ReorderedPropertiesAreSame()
        {
            var a = new TwoProp {Prop1 = "val1", Prop2 = "val2"};
            var b = new TwoPropRev { Prop1 = "val1", Prop2 = "val2" };

            var result = DoCompare(a, b);

            result.AreSame.ShouldBe(true);
        }




        internal class ScoreClass
        {
            public int Score { get; set; }
        }



        [Test]
        public void HintedPropertyIsAllowedToVary()
        {
            var a = new ScoreClass { Score = 100 };
            var b = new ScoreClass { Score = 99 };

            // TODO - should have a nice, fluent interface to build caveats
            var caveats = new[] { new VarianceCaveat(DiffPath.FromJsonPath("$.score"), 2) };

            var result = DoCompare(a, b, caveats);

            result.AreSame.ShouldBe(true);
        }



        [Test]
        public void PropertyOutsideVarianceIsNotSame()
        {
            const string a = "{ \"score\": 100 }";
            const string b = "{ \"score\": 110 }";

            var caveats = new[] { new VarianceCaveat(DiffPath.FromJsonPath("$.score"), 2) };

            var result = DoCompare(a, b, caveats);

            result.AreSame.ShouldBe(false);
        }



        #region Helpers

        protected string AsJson(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(obj, settings);
        }


        protected string AsXml(object obj)
        {
            XmlSerializer cereal = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                cereal.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        #endregion
    }
}
