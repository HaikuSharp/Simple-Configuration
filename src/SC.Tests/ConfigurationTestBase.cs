using SC.Abstraction;
using SC.Extensions;
using System;
using System.Threading.Tasks;

namespace SC.Tests;

[TestClass]
public abstract class ConfigurationTestBase
{
    [TestMethod]
    public void OptionTest()
    {
        var configuration = GetConfiguration();

        DefaultTest(configuration, "SomeSection:SomeValue");
    }

    [TestMethod]
    public void SectionTest()
    {
        var configuration = GetConfiguration();

        ConfigurationTest(configuration);

        var section = configuration.GetSection("SomeSection");

        DefaultTest(section, "SomeValue");
    }

    [TestMethod]
    public async Task OptionAsyncTest()
    {
        var configuration = GetConfiguration();

        await DefaultAsyncTest(configuration, "SomeSection:SomeValue");
    }

    [TestMethod]
    public async Task SectionAsyncTest()
    {
        var configuration = GetConfiguration();

        ConfigurationTest(configuration);

        var section = configuration.GetSection("SomeSection");

        await DefaultAsyncTest(section, "SomeValue");
    }

    protected static async Task DefaultAsyncTest(IConfiguration configuration, string optionPath)
    {
        ConfigurationTest(configuration);
        await LoadAsyncTest(configuration);

        var option = configuration.GetOption<int>(optionPath);

        DefaultOptionTest(option);
        // await SaveAsyncTest(configuration);
    }

    protected static void DefaultTest(IConfiguration configuration, string optionPath)
    {
        ConfigurationTest(configuration);

        LoadTest(configuration);

        var option = configuration.GetOption<int>(optionPath);

        DefaultOptionTest(option);
        // SaveTest(configuration);
    }

    protected static void DefaultOptionTest(IConfigurationOption<int> option)
    {
        OptionTest(option);
        ChangeTest(option);
    }

    protected static void LoadTest(IConfiguration configuration) => configuration.Load();

    protected static void SaveTest(IConfiguration configuration) => configuration.Save();

    protected static async Task LoadAsyncTest(IConfiguration configuration) => await configuration.LoadAsync();

    protected static async Task SaveAsyncTest(IConfiguration configuration) => await configuration.SaveAsync();

    protected static void ChangeTest(IConfigurationOption<int> option) => option.Value = 151;

    protected static void OptionTest(IConfigurationOption<int> option)
    {
        Assert.IsNotNull(option);
        Assert.AreEqual(10, option.Value);
    }

    protected static void ConfigurationTest(IConfiguration configuration) => Assert.IsNotNull(configuration);

    protected IConfiguration GetConfiguration() => GetConfigurationSource().CreateConfiguration(GetConfigurationSettings());

    protected virtual IConfigurationSettings GetConfigurationSettings() => DefaultConfigurationSettings.Default;

    protected abstract IConfigurationSource GetConfigurationSource();
}
