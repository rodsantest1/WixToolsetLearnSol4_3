using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    internal class Program
    {
        static ServiceProvider _serviceProvider;

        static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(AddConfiguration());
        }

        static IConfiguration AddConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

#if DEV
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#elif PROD
            builder.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif

            return builder.Build();
        }

        static void Main(string[] args)
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var appsettings = _serviceProvider.GetService<IConfiguration>();

            var configHello = $"Hello {appsettings?.GetValue<string>("hello")}!";

            Console.WriteLine(configHello);

            Console.ReadLine();
        }
    }
}
