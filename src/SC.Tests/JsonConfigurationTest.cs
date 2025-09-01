using SC.Abstraction;
using SC.Newtonsoft.JSON;
using SC.Tests.Models;
using System;
using System.IO;

namespace SC.Tests;

[TestClass]
public sealed class JsonConfigurationTest : ConfigurationTestBase
{
    [TestMethod]
    public void JsonModelTest()
    {
        var configuration = GetConfiguration();

        ConfigurationTest(configuration);
        LoadTest(configuration);
        ModelOptionTest(configuration);
    }

    private static void ModelOptionTest(IConfiguration configuration)
    {
        var option = configuration.GetOption<SomeSectionModel>("SomeSection");

        Assert.IsNotNull(option);
        Assert.IsNotNull(option.Value);
        Assert.AreEqual(10, option.Value.SomeValue);

        try
        {
            var intOption = configuration.GetOption<int>("SomeSection:SomeValue");

            Assert.IsNull(intOption);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected override IConfigurationSource GetConfigurationSource() => new JsonFileConfigurationSource(Path.Combine("..", "test.json"));
}
