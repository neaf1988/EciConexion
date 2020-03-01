using Common.Utils;
using ECIConexion.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ECIConexion.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider sp = serviceCollection.BuildServiceProvider();
            var repositoryOptions = sp.GetService <IOptions<ECIConexionSettings>>();
            var settings = repositoryOptions.Value;
            string url = $"https://www.googleapis.com/customsearch/v1?key={settings.searchAPISecurity.Key}&cx={settings.searchAPISecurity.CX}&q=Fabio+Enrique+Quintero+DiazGranados";

            using (IRestClient<JObject> rc = new RestClient<JObject>(url))
            {
                var returnable = rc.GETRequestAsync().Result;
            }
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .Build();
            Console.WriteLine(configuration.GetConnectionString("Storage"));
            services.Configure<ECIConexionSettings>(configuration);

        }
    }
}
