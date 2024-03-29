﻿using System.Collections;

namespace ManagedCode.IdGenerator.ConsistentHashing
{
    /// <summary>
    /// Represents a collection of non-overlapping hash ranges.
    /// </summary>
    public class HashRangeCollection : IEnumerable<HashRange>
    {
        private readonly List<HashRange> collection;

        /// <summary>
        /// Create a collection of hash ranges. The ranges must be
        /// non-overlapping.
        /// </summary>
        /// <param name="ranges">The hash ranges.</param>
        public HashRangeCollection(IEnumerable<HashRange> ranges)
        {
            this.collection = new List<HashRange>(ranges);
            this.collection.Sort(CompareByStartExclusive);
            this.collection.TrimExcess();
        }

        /// <summary>
        /// Checks whether the specified hash falls within any range in the collection.
        /// </summary>
        /// <param name="hash">The hash to check.</param>
        /// <returns>True if the hash is within a range in the collection and false otherwise.</returns>
        public bool Contains(uint hash)
        {
            int start = 0;
            int end = this.collection.Count - 1;

            while (start <= end)
            {
                int mid = start + ((end - start) / 2);
                int cmp = hash.CompareTo(this.collection[mid].StartExclusive + 1);

                if (cmp == 0)
                {
                    return true;
                }
                else if (cmp < 0)
                {
                    end = mid - 1;
                }
                else
                {
                    if (this.collection[mid].Contains(hash))
                    {
                        return true;
                    }

                    start = mid + 1;
                }
            }

            if (start == this.collection.Count)
            {
                return false;
            }

            // Start may be 0 which means we also need to check the last 
            // range in case it wraps around.
            return this.collection[start].Contains(hash) || this.collection[this.collection.Count - 1].Contains(hash);
        }

        private static int CompareByStartExclusive(HashRange x, HashRange y)
        {
            return x.StartExclusive.CompareTo(y.StartExclusive);
        }

        /// <summary>
        /// Returns an enumerator that iterates over all the <see cref="HashRange"/>s in the collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<HashRange> GetEnumerator()
        {
            return ((IEnumerable<HashRange>)collection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HashRange>)collection).GetEnumerator();
        }
    }
}
