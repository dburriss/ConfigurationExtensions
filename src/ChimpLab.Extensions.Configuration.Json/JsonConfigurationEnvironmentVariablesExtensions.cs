using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace ChimpLab.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for adding <see cref="JsonConfigurationProvider"/>.
    /// </summary>
    public static class JsonConfigurationEnvironmentVariablesExtensions
    {
        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="name"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="name">Name of environment variable containing path to file 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFileFromEnvironmentVariable(this IConfigurationBuilder builder, string name)
        {
            return AddJsonFileFromEnvironmentVariable(builder, provider: null, name: name, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="name"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="name">Name of environment variable containing path to file
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFileFromEnvironmentVariable(this IConfigurationBuilder builder, string name, bool optional)
        {
            return AddJsonFileFromEnvironmentVariable(builder, provider: null, name: name, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the JSON configuration provider at <paramref name="name"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="name">Name of environment variable containing path to file
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFileFromEnvironmentVariable(this IConfigurationBuilder builder, string name, bool optional, bool reloadOnChange)
        {
            return AddJsonFileFromEnvironmentVariable(builder, provider: null, name: name, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
        /// <param name="name">Name of environment variable containing path to file
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonFileFromEnvironmentVariable(this IConfigurationBuilder builder, IFileProvider provider, string name, bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var path = Environment.GetEnvironmentVariable(name) ?? "";

            if (string.IsNullOrEmpty(path))
            {
                return builder;
            }

            if(!File.Exists(path))
            {
                return builder;
            }

            var isRooted = Path.IsPathRooted(path);
            return isRooted ? 
                builder.AddJsonFile(provider, path, optional, reloadOnChange) : 
                builder.AddJsonFileFromAbsolutePath(provider, path, optional, reloadOnChange);
        }
    }
}
