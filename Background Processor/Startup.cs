using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Background_Processor.Startup))]
namespace Background_Processor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddAzureClients(clientsBuilder =>
            {
                clientsBuilder.AddServiceBusClient(configuration["ServiceBus"]);
            });

            builder.Services.AddDbContext<BackgroundProcessorDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("SQL")));
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var configuration = builder.ConfigurationBuilder.Build();

            builder.ConfigurationBuilder.AddAzureKeyVault(
                new Uri($"https://{configuration["KeyVaultName"]}.vault.azure.net/"),
                new DefaultAzureCredential(),
                new AzureKeyVaultConfigurationOptions());
        }
    }
}
