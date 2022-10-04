using Azure.Messaging.ServiceBus;
using Azure_Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace Background_Processor
{
    public class ReceiveMessageTransactionFunction
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ReceiveMessageTransactionFunction(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        [FunctionName("ReceiveMessageTransactionFunction")]
        public async Task<Transaction> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var receiver = _serviceBusClient.CreateReceiver("transactions");

            var message = await receiver.ReceiveMessageAsync();

            await receiver.CompleteMessageAsync(message);

            return JsonSerializer.Deserialize<Transaction>(message.Body.ToString());
        }
    }
}
