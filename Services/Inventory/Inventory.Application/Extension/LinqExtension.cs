using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Extension
{
    internal static class LinqExtension
    {
        internal static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> leftSequence,
            IEnumerable<TRight> rightSequence,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            var leftOuterJoin = from left in leftSequence
                                join right in rightSequence
                                on leftKeySelector(left) equals rightKeySelector(right) into temp
                                from right in temp.DefaultIfEmpty()
                                select resultSelector(left, right);

            var rightOuterJoin = from right in rightSequence
                                 join left in leftSequence
                                 on rightKeySelector(right) equals leftKeySelector(left) into temp
                                 from left in temp.DefaultIfEmpty()
                                 select resultSelector(left, right);

            return leftOuterJoin.Union(rightOuterJoin);
        }
    }
}
