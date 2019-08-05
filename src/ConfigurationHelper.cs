using Microsoft.Extensions.Configuration;

namespace AzureWebMonitor.Test
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {            
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static KavaDocsConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new KavaDocsConfiguration();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("KavaDocs")
                .Bind(configuration);

            return configuration;
        }
    }
}