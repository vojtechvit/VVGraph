using System;
using System.Collections;
using System.Collections.Generic;

namespace WebServices.Proxy.UnitTests.Helpers
{
    public static class VVEnumerable
    {
        public static IEnumerable<object[]> Permute(params IEnumerable[] sets)
        {
            if (sets == null)
                throw new ArgumentNullException(nameof(sets));

            var nonexcludedSets = new List<IEnumerable>();

            foreach (var set in sets)
            {
                if (set != null)
                {
                    nonexcludedSets.Add(set);
                }
            }

            if (nonexcludedSets.Count == 0)
            {
                return new object[0][];
            }

            var results = new List<object[]>();
            var path = new object[nonexcludedSets.Count];

            PermuteSubset(results, nonexcludedSets, path, 0);

            return results;
        }

        private static void PermuteSubset(List<object[]> results, IList<IEnumerable> sets, object[] path, int position)
        {
            foreach (var member in sets[position])
            {
                path[position] = member;

                if (position != sets.Count - 1)
                {
                    PermuteSubset(results, sets, path, position + 1);
                }
                else
                {
                    var result = new object[sets.Count];
                    path.CopyTo(result, 0);
                    results.Add(result);
                }
            }
        }
    }
}