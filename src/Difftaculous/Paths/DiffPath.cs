﻿
using System.Collections.Generic;
using Difftaculous.ZModel;


namespace Difftaculous.Paths
{
    public interface IDiffPath
    {
        // TODO - obsolete
        bool Matches(IToken token);

        string AsJsonPath { get; }
    }



    public class DiffPath : IDiffPath
    {

        private DiffPath()
        {
            Filters = new List<PathFilter>();
        }


        internal List<PathFilter> Filters { get; private set; }



        public static DiffPath FromJsonPath(string path)
        {
            return new DiffPath
            {
                AsJsonPath = path,
                Filters = JsonPathParser.Parse(path)
            };
        }


        public static DiffPath FromToken(IToken token)
        {
            return token.Path;
        }



        public string AsJsonPath { get; private set; }


        public override bool Equals(object obj)
        {
            DiffPath other = obj as DiffPath;

            if (other == null)
            {
                return false;
            }

            return AsJsonPath == other.AsJsonPath;
        }


        public override int GetHashCode()
        {
            return AsJsonPath.GetHashCode();
        }


        public override string ToString()
        {
            return AsJsonPath;
        }



        // TODO - Get rid of matches - should use SelectTokens instead!
        public bool Matches(IToken token)
        {
            return AsJsonPath.Equals(token.Path.AsJsonPath);
        }



        internal IEnumerable<ZToken> Evaluate(ZToken t)
        {
            return Evaluate(Filters, t);
        }

        internal static IEnumerable<ZToken> Evaluate(List<PathFilter> filters, ZToken t)
        {
            IEnumerable<ZToken> current = new[] { t };
            foreach (PathFilter filter in filters)
            {
                current = filter.ExecuteFilter(current);
            }

            return current;
        }
    }
}
