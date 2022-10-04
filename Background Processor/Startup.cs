using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Background_Processor.Startup))]
namespace Background_Processor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureClients(clientsBuilder =>
            {
                var configuration = builder.GetContext().Configuration;

                clientsBuilder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
            });
        }
    }
}
