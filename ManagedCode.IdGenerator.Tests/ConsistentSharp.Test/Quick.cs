using System;
using Xunit;

namespace ManagedCode.IdGenerator.Tests.ConsistentSharp.Test
{
    // https://golang.org/src/testing/quick/quick.go
    internal static class Quick
    {
        internal static void Check<T>(Func<T, bool> f, int maxCount = 100) where T : class
        {
            for (var i = 0; i < maxCount; i++)
            {
                Assert.True(f(Random<T>()));
            }
        }

        private static T Random<T>() where T : class
        {
            const int complexSize = 50;

            if (typeof(T) == typeof(string))
            {
                var numChars = Rand.Intn(complexSize);
                var codePoints = new char[numChars];

                for (var i = 0; i < numChars; i++)
                {
                    codePoints[i] = (char) Rand.Intn(0x10ffff);
                }

                return new string(codePoints) as T;
            }

            throw new NotImplementedException();
        }
    }
}