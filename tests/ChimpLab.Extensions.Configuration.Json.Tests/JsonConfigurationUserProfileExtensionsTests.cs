using System;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;

namespace ChimpLab.Extensions.Configuration.Json.Tests
{
    public class JsonConfigurationUserProfileExtensionsTests
    {
        [Fact]
        public void AddJsonFileFromUserProfile_BuildWithFile_ContainsConfigKey()
        {
            string path = "test.json";
            var userDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
            path = System.IO.Path.Combine(userDirectory, path);
            var contents = @"{ 'Database' : 'Acceptance Db' }";
            IConfigurationBuilder sut = new ConfigurationBuilder();
            IConfiguration config = null;

            using (var t = new ManagedConfigFile(path, contents))
            {
                sut.AddJsonFileFromUserProfile(path);
                config = sut.Build();
            }

            Assert.Contains("Database", config.AsEnumerable().Select(k => k.Key));
        }
    }
}
