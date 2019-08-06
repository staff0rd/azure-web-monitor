using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AzureWebMonitor.Test
{
    public static class ConfigurationHelper
    {
        public static string WorkingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static IConfigurationRoot GetIConfigurationRoot()
        {            
            return new ConfigurationBuilder()
                .SetBasePath(WorkingDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static ApplicationInsightsConfig GetApplicationConfiguration()
        {
            var configuration = new ApplicationInsightsConfig();

            var iConfig = GetIConfigurationRoot();

            iConfig
                .GetSection("ApplicationInsights")
                .Bind(configuration);

            return configuration;
        }
    }
}