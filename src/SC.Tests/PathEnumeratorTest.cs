using SC.Abstraction;
using System;

namespace SC.Tests;

[TestClass]
public sealed class PathEnumeratorTest
{
    [TestMethod]
    public void EnumeratorTest()
    {
        int count = 0;

        foreach(string pathPart in new ConfigurationPathEnumerator("SomeSection:SomeValue", ":"))
        {
            Console.WriteLine(pathPart);
            count++;
        }

        Assert.AreEqual(2, count);
    }
}
