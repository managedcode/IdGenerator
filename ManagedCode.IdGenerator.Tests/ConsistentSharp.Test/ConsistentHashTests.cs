using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagedCode.IdGenerator.ConsistentSharp;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.ConsistentSharp.Test
{
    // https://github.com/stathat/consistent/blob/master/consistent_test.go

    public class ConsistentHashTests
    {
        private static bool IsSorted<T>(T[] arr) where T : IComparable
        {
            for (var i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1].CompareTo(arr[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        [Fact]
        public void TestNew()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            Assert.Equal(20, x.NumberOfReplicas);
        }

        [Fact]
        public void TestAdd()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Add("abcdefg");

            Assert.Equal(20, x.Circle.Count);
            Assert.Equal(20, x.SortedHashes.Length);

            Assert.True(IsSorted(x.SortedHashes));

            x.Add("qwer");

            Assert.Equal(40, x.Circle.Count);
            Assert.Equal(40, x.SortedHashes.Length);

            Assert.True(IsSorted(x.SortedHashes));
        }

        [Fact]
        public void TestRemove()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Add("abcdefg");
            x.Remove("abcdefg");

            Assert.Equal(0, x.Circle.Count);
            Assert.Equal(0, x.SortedHashes.Length);
        }

        [Fact]
        public void TestRemoveNonExisting()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Add("abcdefg");
            x.Remove("abcdefghijk");
            Assert.Equal(20, x.Circle.Count);
        }

        [Fact]
        public void TestGetEmpty()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            try
            {
                x.Get("asdfsadfsadf");
            }
            catch (EmptyCircleException)
            {
                return;
            }

            Assert.Fail("expected error");
        }


        [Fact]
        public void TestGetSingle()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");

            Quick.Check<string>(s =>
            {
                var y = x.Get(s);
                return y == "abcdefg";
            });
        }

        [Fact]
        public void TestGetMultiple()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            foreach (var gmtest in new[]
            {
                // new[] { in, out }
                new[] {"ggg", "abcdefg"},
                new[] {"hhh", "opqrstu"},
                new[] {"iiiii", "hijklmn"}
            })
            {
                Assert.Equal(gmtest[1], x.Get(gmtest[0]));
            }
        }

        [Fact]
        public void TestGetMultipleQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            Quick.Check<string>(s =>
            {
                var y = x.Get(s);
                return y == "abcdefg" || y == "hijklmn" || y == "opqrstu";
            });
        }

        [Fact]
        public void TestGetMultipleRemove()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            foreach (var gmtest in new[]
            {
                // new[] { in, out }
                new[] {"ggg", "abcdefg"},
                new[] {"hhh", "opqrstu"},
                new[] {"iiiii", "hijklmn"}
            })
            {
                Assert.Equal(gmtest[1], x.Get(gmtest[0]));
            }

            x.Remove("hijklmn");

            foreach (var gmtest in new[]
            {
                // new[] { in, out }
                new[] {"ggg", "abcdefg"},
                new[] {"hhh", "opqrstu"},
                new[] {"iiiii", "opqrstu"}
            })
            {
                Assert.Equal(gmtest[1], x.Get(gmtest[0]));
            }
        }

        [Fact]
        public void TestGetMultipleRemoveQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            x.Remove("opqrstu");

            Quick.Check<string>(s =>
            {
                var y = x.Get(s);
                return y == "abcdefg" || y == "hijklmn";
            });
        }

        [Fact]
        public void TestGetTwo()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            var (a, b) = x.GetTwo("99999999");

            Assert.NotEqual(a, b);
            Assert.Equal("abcdefg", a);
            Assert.Equal("hijklmn", b);
        }

        [Fact]
        public void TestGetTwoQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            Quick.Check<string>(s =>
            {
                var (a, b) = x.GetTwo(s);

                if (a == b)
                {
                    return false;
                }

                if (a != "abcdefg" && a != "hijklmn" && a != "opqrstu")
                {
                    return false;
                }

                if (b != "abcdefg" && b != "hijklmn" && b != "opqrstu")
                {
                    return false;
                }

                return true;
            });
        }

        [Fact]
        public void TestGetTwoOnlyTwoQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");

            Quick.Check<string>(s =>
            {
                var (a, b) = x.GetTwo(s);

                if (a == b)
                {
                    return false;
                }

                if (a != "abcdefg" && a != "hijklmn")
                {
                    return false;
                }

                if (b != "abcdefg" && b != "hijklmn")
                {
                    return false;
                }

                return true;
            });
        }

        [Fact]
        public void TestGetTwoOnlyOneInCircle()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");

            var (a, b) = x.GetTwo("99999999");
            Assert.NotEqual(a, b);

            Assert.Equal("abcdefg", a);
            Assert.Equal(default(string), b);
        }


        [Fact]
        public void TestGetN()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            var members = x.GetN("9999999", 3).ToArray();

            Assert.Equal(3, members.Length);
            Assert.Equal("opqrstu", members[0]);
            Assert.Equal("abcdefg", members[1]);
            Assert.Equal("hijklmn", members[2]);
        }

        [Fact]
        public void TestGetNLess()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            var members = x.GetN("99999999", 2).ToArray();

            Assert.Equal(2, members.Length);
            Assert.Equal("abcdefg", members[0]);
            Assert.Equal("hijklmn", members[1]);
        }

        [Fact]
        public void TestGetNMore()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            var members = x.GetN("9999999", 5).ToArray();

            Assert.Equal(3, members.Length);
            Assert.Equal("opqrstu", members[0]);
            Assert.Equal("abcdefg", members[1]);
            Assert.Equal("hijklmn", members[2]);
        }

        [Fact]
        public void TestGetNQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            Quick.Check<string>(s =>
            {
                var members = x.GetN(s, 3).ToArray();

                if (members.Length != 3)
                {
                    return false;
                }

                var set = new HashSet<string>();

                foreach (var member in members)
                {
                    if (set.Contains(member))
                    {
                        return false;
                    }

                    set.Add(member);

                    if (member != "abcdefg" && member != "hijklmn" && member != "opqrstu")
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        [Fact]
        public void TestGetNLessQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            Quick.Check<string>(s =>
            {
                var members = x.GetN(s, 2).ToArray();

                if (members.Length != 2)
                {
                    return false;
                }

                var set = new HashSet<string>();

                foreach (var member in members)
                {
                    if (set.Contains(member))
                    {
                        return false;
                    }

                    set.Add(member);

                    if (member != "abcdefg" && member != "hijklmn" && member != "opqrstu")
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        [Fact]
        public void TestGetNMoreQuick()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abcdefg");
            x.Add("hijklmn");
            x.Add("opqrstu");

            Quick.Check<string>(s =>
            {
                var members = x.GetN(s, 5).ToArray();

                if (members.Length != 3)
                {
                    return false;
                }

                var set = new HashSet<string>();

                foreach (var member in members)
                {
                    if (set.Contains(member))
                    {
                        return false;
                    }

                    set.Add(member);

                    if (member != "abcdefg" && member != "hijklmn" && member != "opqrstu")
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        [Fact]
        public void TestSet()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();

            x.Add("abc");
            x.Add("def");
            x.Add("ghi");

            {
                x.Set(new[] {"jkl", "mno"});

                Assert.Equal(2, x.Count);

                var (a, b) = x.GetTwo("qwerqwerwqer");
                Assert.True(a == "jkl" || a == "mno");
                Assert.True(b == "jkl" || b == "mno");
                Assert.NotEqual(a, b);
            }

            {
                x.Set(new[] {"pqr", "mno"});

                Assert.Equal(2, x.Count);

                var (a, b) = x.GetTwo("qwerqwerwqer");
                Assert.True(a == "pqr" || a == "mno");
                Assert.True(b == "pqr" || b == "mno");
                Assert.NotEqual(a, b);
            }
        }

        [Fact]
        public void TestAddCollision()
        {
            // These two strings produce several crc32 collisions after "|i" is
            // appended added by Consistent.eltKey.
            const string s1 = "abear";
            const string s2 = "solidiform";

            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Add(s1);
            x.Add(s2);

            var elt1 = x.Get(s1);

            var y = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            y.Add(s2);
            y.Add(s1);

            var elt2 = y.Get(s1);

            Assert.Equal(elt1, elt2);
        }

        [Fact]
        public void TestConcurrentGetSet()
        {
            var x = new ManagedCode.IdGenerator.ConsistentSharp.ConsistentHash();
            x.Set(new[] {"abc", "def", "ghi", "jkl", "mno"});

            var tasks = new List<Task>();

            for (var i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        x.Set(new[] {"abc", "def", "ghi", "jkl", "mno"});
                        await Task.Delay(TimeSpan.FromMilliseconds(Rand.Intn(10)));

                        x.Set(new[] {"pqr", "stu", "vwx"});
                        await Task.Delay(TimeSpan.FromMilliseconds(Rand.Intn(10)));
                    }
                }));
            }

            for (var i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (var j = 0; j < 1000; j++)
                    {
                        var a = x.Get("xxxxxxx");

                        Assert.True(a == "def" || a == "vwx");

                        await Task.Delay(TimeSpan.FromMilliseconds(Rand.Intn(10)));
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}