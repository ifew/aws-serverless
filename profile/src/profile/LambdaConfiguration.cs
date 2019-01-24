using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace list_profile
{
    public static class LambdaConfiguration
    {
        private static IConfigurationRoot instance = null;
        public static IConfigurationRoot Instance
        {
            get 
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var basePath = Directory.GetCurrentDirectory();

                return instance ?? (instance = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .Build());
            }
        }
    }
}