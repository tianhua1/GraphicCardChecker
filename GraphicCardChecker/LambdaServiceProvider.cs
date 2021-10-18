using GraphicCardChecker.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace GraphicCardChecker
{
    public static class LambdaServiceProvider
    {
        public static ServiceProvider Build() 
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            AddBaseConfiguration(builder, services, environment, out IConfiguration configuration);
            AddAWSConfiguration(services, configuration, out AWSConfiguration AWSConfiguration);
            AddHandler(services);
            return services.BuildServiceProvider();
        }

        private static void AddHandler(ServiceCollection services) 
        {
            services.AddTransient<IHandler, Handler>();
        }

        private static void AddBaseConfiguration(ConfigurationBuilder builder, ServiceCollection services, string environment, out IConfiguration configuration) 
        {
            configuration = builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
            services.AddSingleton(configuration);
        }

        private static void AddAWSConfiguration(ServiceCollection service, IConfiguration configuration, out AWSConfiguration AWSConfiguration) 
        {
            AWSConfiguration = new AWSConfiguration();
            configuration.GetSection(nameof(AWSConfiguration)).Bind(AWSConfiguration);

            service.AddSingleton(AWSConfiguration);
        
        }
    }
}
