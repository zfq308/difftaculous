﻿
using System;
using System.Collections.Generic;
using Difftaculous.Paths.Expressions;
using Difftaculous.ZModel;


namespace Difftaculous.Paths
{
    internal class QueryFilter : PathFilter
    {
        public QueryExpression Expression { get; set; }


        public override IEnumerable<ZToken> ExecuteFilter(IEnumerable<ZToken> current)
        {
#if false
            foreach (JToken t in current)
            {
                foreach (JToken v in t)
                {
                    if (Expression.IsMatch(v))
                        yield return v;
                }
            }
#endif

            throw new NotImplementedException();
        }

    }
}
