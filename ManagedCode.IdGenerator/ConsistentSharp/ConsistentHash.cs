namespace ManagedCode.IdGenerator.ConsistentSharp
{
    /// from https://github.com/stathat/consistent/blob/master/consistent.go
    public partial class ConsistentHash : IDisposable
    {
        private readonly Dictionary<uint, string> _circle = new Dictionary<uint, string>();
        private readonly Dictionary<string, bool> _members = new Dictionary<string, bool>();
        private readonly ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly IHashAlgorithm _hashAlgorithm;

        private long _count;

        private uint[] _sortedHashes = new uint[0];

        public int NumberOfReplicas { get; set; } = 20;

        public ConsistentHash(IHashAlgorithm hashAlgorithm = null) {
            _hashAlgorithm = hashAlgorithm ?? new Crc32HashAlgorithm();
        }
        
        public IEnumerable<string> Members
        {
            get
            {
                _rwlock.EnterReadLock();

                try
                {
                    return _members.Keys.ToArray();
                }
                finally
                {
                    _rwlock.ExitReadLock();
                }
            }
        }

        public void Dispose()
        {
            _rwlock.Dispose();
        }


        public void Add(string elt)
        {
            if (elt == null)
            {
                throw new ArgumentNullException(nameof(elt));
            }

            _rwlock.EnterWriteLock();

            try
            {
                _Add(elt);
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        private void _Add(string elt)
        {
            for (var i = 0; i < NumberOfReplicas; i++)
            {
                _circle[_hashAlgorithm.HashKey(EltKey(elt, i))] = elt;
            }

            _members[elt] = true;
            UpdateSortedHashes();
            _count++;
        }

        public void Remove(string elt)
        {
            if (elt == null)
            {
                throw new ArgumentNullException(nameof(elt));
            }

            _rwlock.EnterWriteLock();
            try
            {
                _Remove(elt);
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        private void _Remove(string elt)
        {
            for (var i = 0; i < NumberOfReplicas; i++)
            {
                _circle.Remove(_hashAlgorithm.HashKey(EltKey(elt, i)));
            }

            _members.Remove(elt);
            UpdateSortedHashes();
            _count--;
        }

        public void Set(IEnumerable<string> elts)
        {
            if (elts == null)
            {
                throw new ArgumentNullException(nameof(elts));
            }

            _Set(elts.ToArray());
        }

        private void _Set(string[] elts)
        {
            _rwlock.EnterWriteLock();
            try
            {
                foreach (var k in _members.Keys.ToArray())
                {
                    var found = elts.Any(v => k == v);

                    if (!found)
                    {
                        _Remove(k);
                    }
                }

                foreach (var v in elts)
                {
                    if (_members.ContainsKey(v))
                    {
                        continue;
                    }

                    _Add(v);
                }
            }

            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        public string Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _rwlock.EnterReadLock();

            try
            {
                if (_count == 0)
                {
                    throw new EmptyCircleException();
                }

                var key = _hashAlgorithm.HashKey(name);

                var i = Search(key);

                return _circle[_sortedHashes[i]];
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

#if NETSTANDARD1_0
        public (string, string) GetTwo(string name)
#else
        public Tuple<string, string> GetTwo(string name)
#endif
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _rwlock.EnterReadLock();

            try
            {
                if (_count == 0)
                {
                    throw new EmptyCircleException();
                }

                var key = _hashAlgorithm.HashKey(name);

                var i = Search(key);

                var a = _circle[_sortedHashes[i]];

                if (_count == 1)
                {
#if NETSTANDARD1_0
                    return (a, default(string));
#else
                    return new Tuple<string, string>(a, default(string));
#endif
                }

                var start = i;

                var b = default(string);

                for (i = start + 1; i != start; i++)
                {
                    if (i >= _sortedHashes.Length)
                    {
                        i = 0;
                    }

                    b = _circle[_sortedHashes[i]];

                    if (b != a)
                    {
                        break;
                    }
                }

#if NETSTANDARD1_0
                return (a, b);
#else
                return new Tuple<string, string>(a, b);
#endif
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

        public IEnumerable<string> GetN(string name, int n)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_count < n)
            {
                n = (int) _count;
            }

            _rwlock.EnterReadLock();

            try
            {
                if (_count == 0)
                {
                    throw new EmptyCircleException();
                }


                var key = _hashAlgorithm.HashKey(name);
                var i = Search(key);
                var start = i;
                var res = new List<string>();
                var elem = _circle[_sortedHashes[i]];

                res.Add(elem);
                if (res.Count == n)
                {
                    return res;
                }

                for (i = start + 1; i != start; i++)
                {
                    if (i >= _sortedHashes.Length)
                    {
                        i = 0;
                    }

                    elem = _circle[_sortedHashes[i]];

                    if (!res.Contains(elem))
                    {
                        res.Add(elem);
                    }

                    if (res.Count == n)
                    {
                        break;
                    }
                }

                return res;
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

        private int Search(uint key)
        {
            var i = BinarySearch(_sortedHashes.Length, x => _sortedHashes[x] > key);

            if (i >= _sortedHashes.Length)
            {
                i = 0;
            }

            return i;
        }

        /// Search uses binary search to find and return the smallest index i in [0, n) at which f(i) is true
        /// golang sort.Search
        private static int BinarySearch(int n, Func<int, bool> f)
        {
            var s = 0;
            var e = n;

            while (s < e)
            {
                var m = s + (e - s) / 2;

                if (!f(m))
                {
                    s = m + 1;
                }
                else
                {
                    e = m;
                }
            }

            return s;
        }

        private void UpdateSortedHashes()
        {
            var hashes = _circle.Keys.ToArray();
            Array.Sort(hashes);
            _sortedHashes = hashes;
        }

        private static string EltKey(string elt, int idx)
        {
            return $"{idx}{elt}";
        }
    }
}