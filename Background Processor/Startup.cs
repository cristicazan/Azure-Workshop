using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                clientsBuilder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
            });

            builder.Services.AddDbContext<BackgroundProcessorDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("SQL")));
        }
    }
}
