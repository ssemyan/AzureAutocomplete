using System;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AddressValidatorFunc.Startup))]

namespace AddressValidatorFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register the CosmosClient as a Singleton
            builder.Services.AddSingleton((s) => {
                string connectionString = Environment.GetEnvironmentVariable("CosmosSqlConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException("Please specify a valid CosmosSqlConnectionString in the appSettings.json file or your Azure Functions Settings.");
                }

                CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(connectionString);
                return configurationBuilder.Build();
            });
        }
    }
}