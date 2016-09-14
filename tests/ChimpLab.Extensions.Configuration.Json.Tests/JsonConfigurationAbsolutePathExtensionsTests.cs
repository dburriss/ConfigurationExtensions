using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xunit;
using ChimpLab.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using PhilosophicalMonkey;

namespace ChimpLab.Extensions.Configuration.Json.Tests
{
    public class JsonConfigurationAbsolutePathExtensionsTests
    {
        private const string doesNotExistPath = "c:\\doesnotexist.json";
        private string _fileToCreate = "";
        public JsonConfigurationAbsolutePathExtensionsTests()
        {
            var tempPath = System.IO.Path.GetTempPath();
            _fileToCreate = System.IO.Path.Combine(tempPath, "test.json");
        }
        [Fact]
        public void AddJsonFileFromAbsolutePath_WithNoPath_ThrowsArgumentException()
        {
            string path = null;
            IConfigurationBuilder sut = new ConfigurationBuilder();

            Assert.Throws<ArgumentException>(() => sut.AddJsonFileFromAbsolutePath(path));
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_WithPath_AddsProvider()
        {
            string path = doesNotExistPath;
            IConfigurationBuilder sut = new ConfigurationBuilder();

            sut.AddJsonFileFromAbsolutePath(path);

            Assert.NotEmpty(sut.Sources);
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_WithPath_PathSetToFilename()
        {
            string path = doesNotExistPath;
            IConfigurationBuilder sut = new ConfigurationBuilder();

            sut.AddJsonFileFromAbsolutePath(path);
            var source = sut.Sources.First();
            string setPath = GetValue<string>(source, "Path");
            Assert.Equal("doesnotexist.json", setPath);
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_WithPath_SourceIsJsonConfigurationSource()
        {
            string path = doesNotExistPath;
            IConfigurationBuilder sut = new ConfigurationBuilder();

            sut.AddJsonFileFromAbsolutePath(path);
            var source = sut.Sources.First();
            Assert.IsType<JsonConfigurationSource>(source);
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_BuildWithPathThatExists_BuildsAConfig()
        {
            string path = _fileToCreate;
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            IConfigurationBuilder sut = new ConfigurationBuilder();
            IConfiguration config = null;
            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromAbsolutePath(path);
                config = sut.Build();
            }

            Assert.NotNull(config);
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_BuildWithPathThatExists_ContainsDatabaseConfig()
        {
            string path = _fileToCreate;
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            IConfigurationBuilder sut = new ConfigurationBuilder();
            IConfiguration config = null;
            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromAbsolutePath(path);
                config = sut.Build();
            }

            Assert.Contains("Database", config.AsEnumerable().Select(k => k.Key));
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_OptionalWithPathThatDoesNotExist_DoesNotAddFile()
        {
            string path = "c:\\this\\directory\\doesnotexist\\file.json";
            IConfigurationBuilder sut = new ConfigurationBuilder();

            sut.AddJsonFileFromAbsolutePath(path, optional:true);

            Assert.Empty(sut.Sources);
        }

        [Fact]
        public void AddJsonFileFromAbsolutePath_OptionalButReloadOnChangeWithPathThatDoesNotExist_ThrowsException()
        {
            string path = "c:\\this\\directory\\doesnotexist\\file.json";
            IConfigurationBuilder sut = new ConfigurationBuilder();

            Assert.Throws<ArgumentException>(() => sut.AddJsonFileFromAbsolutePath(path, optional:true, reloadOnChange: true));
        }

        private T GetValue<T>(IConfigurationSource source, string propName)
        {
            var type = source.GetType();
            var prop = type.GetProperty(propName);
            var value = prop.GetValue(source);
            return (T)value;
        }
    }
}
