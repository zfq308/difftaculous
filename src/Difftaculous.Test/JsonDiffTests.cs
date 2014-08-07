﻿
using System;
using System.Collections.Generic;
using Difftaculous.Adapters;
using Difftaculous.Caveats;
using Difftaculous.Hints;
using Difftaculous.Results;


namespace Difftaculous.Test
{
    public class JsonDiffTests : AbstractDiffTests
    {
        protected override IDiffResult DoCompare(object a, object b, IEnumerable<ICaveat> caveats, IEnumerable<IHint> hints)
        {
            var jsonA = AsJson(a);
            var jsonB = AsJson(b);

            Console.WriteLine("JSON, A:\n{0}", jsonA);
            Console.WriteLine();
            Console.WriteLine("JSON, B:\n{0}", jsonB);

            var result = DiffEngine.Compare(new JsonAdapter(jsonA), new JsonAdapter(jsonB), caveats, hints);

            Console.WriteLine();
            Console.WriteLine("Result:\n{0}", result);

            return result;
        }
    }
}
