using System;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;
using PhilosophicalMonkey;

namespace ChimpLab.Extensions.Configuration.Json.Tests
{
    public class JsonConfigurationEnvironmentVariablesExtensionsTests
    {
        private const string doesNotExistPath = "c:\\doesnotexist.json";
        private string _fileToCreate = "";
        public JsonConfigurationEnvironmentVariablesExtensionsTests()
        {
            var tempPath = System.IO.Path.GetTempPath();
            _fileToCreate = System.IO.Path.Combine(tempPath, "test.json");
        }

        [Fact]
        public void AddJsonFileFromEnvironmentVariable_WithKeyThatDoesNotExist_DoesNothing()
        {
            string key = "SomeKeyThatDoesNotExist";
            IConfigurationBuilder sut = new ConfigurationBuilder();

            sut.AddJsonFileFromEnvironmentVariable(key);

            Assert.Empty(sut.Sources);
        }

        [Fact]
        public void AddJsonFileFromEnvironmentVariable_WithKeyThatDoesExist_AddsSource()
        {
            string key = "SomeKeyThatDoesNotExist";
            string path = _fileToCreate;
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            Environment.SetEnvironmentVariable(key, path);
            IConfigurationBuilder sut = new ConfigurationBuilder();

            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromEnvironmentVariable(key);
            }
            
            Assert.NotEmpty(sut.Sources);
        }

        [Fact]
        public void AddJsonFileFromEnvironmentVariable_WhenBuild_CreatesConfig()
        {
            string key = "SomeKeyThatDoesNotExist";
            string path = _fileToCreate;
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            Environment.SetEnvironmentVariable(key, path);
            IConfigurationBuilder sut = new ConfigurationBuilder();
            IConfiguration config = null;

            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromEnvironmentVariable(key);
                config = sut.Build();
            }
            Assert.NotNull(config);
        }

        [Fact]
        public void AddJsonFileFromEnvironmentVariable_BuildWithKeyThatDoesExist_ContainsConfigKey()
        {
            string key = "SomeKeyThatDoesNotExist";
            string path = _fileToCreate;
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            Environment.SetEnvironmentVariable(key, path);
            IConfigurationBuilder sut = new ConfigurationBuilder();
            IConfiguration config = null;

            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromEnvironmentVariable(key);
                config = sut.Build();
            }

            Assert.Contains("Database", config.AsEnumerable().Select(k => k.Key));
        }

        //[Fact]
        //public void AddJsonFileFromEnvironmentVariable_WithKeyThatDoesExistButPointsToFolderThatDoesNot_DoesNothing()
        //{
        //    string key = "SomeKeyThatDoesNotExist";
        //    string path = @"C:\path\that\doesnt\exist\nofile.json";
        //    Environment.SetEnvironmentVariable(key, path);
        //    IConfigurationBuilder sut = new ConfigurationBuilder();

        //    sut.AddJsonFileFromEnvironmentVariable(key);

        //    Assert.Empty(sut.Sources);
        //}

        private T GetValue<T>(IConfigurationSource source, string propName)
        {
            var type = source.GetType();
            var prop = Reflect.OnProperties.GetPropertyInformation(type, propName); //type.GetProperty(propName);
            var value = prop.GetValue(source);
            return (T)value;
        }
    }
}
