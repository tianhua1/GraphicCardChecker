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
                        .AddJsonFile(path: "appsettings1.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings1.{environment}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
            var connectionString = configuration["ConnectionString"];
            services.AddSingleton(configuration);
        }
    }
}
