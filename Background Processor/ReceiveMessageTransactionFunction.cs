using Azure.Messaging.ServiceBus;
using Azure_Workshop;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Background_Processor
{
    public class ReceiveMessageTransactionFunction
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly BackgroundProcessorDbContext _backgroundProcessorDb;
        private readonly TelemetryClient _telemetryClient;

        public ReceiveMessageTransactionFunction(ServiceBusClient serviceBusClient, BackgroundProcessorDbContext backgroundProcessorDbContext, TelemetryClient telemetryClient)
        {
            _serviceBusClient = serviceBusClient;
            _backgroundProcessorDb = backgroundProcessorDbContext;
            _telemetryClient = telemetryClient;
        }

        [FunctionName("ReceiveMessageTransactionFunction")]
        public async Task<Transaction> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var receiver = _serviceBusClient.CreateReceiver("transactions");

            _telemetryClient.TrackTrace("Hello from the function");

            var message = await receiver.ReceiveMessageAsync();

            await receiver.CompleteMessageAsync(message);

            var transaction = JsonSerializer.Deserialize<Transaction>(message.Body.ToString());

            await _backgroundProcessorDb.AddAsync(new TransactionDao { Id = Guid.NewGuid(), Date = transaction.Date, Name = transaction.Name, Value = transaction.Value });

            await _backgroundProcessorDb.SaveChangesAsync();

            return transaction;
        }
    }
}
