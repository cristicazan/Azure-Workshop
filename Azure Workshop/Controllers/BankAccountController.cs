using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Azure_Workshop.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BankAccountController : ControllerBase
    {
        private static readonly string[] names = new[]
        {
            "Cecilia",
            "Avery",
            "Sharon",
            "Gael",
            "Mackenzie",
            "Simone",
            "Hamza",
            "Mauricio",
            "Darien",
            "Rafael",
            "Muhammad",
            "Kathy",
        };

        private readonly ServiceBusClient _serviceBusClient;

        public BankAccountController(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            return Enumerable.Range(1, 10).Select(GetRandomTransaction).ToArray();
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageTransaction()
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(GetRandomTransaction()));

            var sender = _serviceBusClient.CreateSender("transactions");

            await sender.SendMessageAsync(message);

            return Ok();
        }

        [HttpGet]
        public async Task<Transaction?> GetMessageTransaction()
        {
            var receiver  = _serviceBusClient.CreateReceiver("transactions");

            var message = await receiver.ReceiveMessageAsync();

            await receiver.CompleteMessageAsync(message);

            return JsonSerializer.Deserialize<Transaction>(message.Body.ToString());
        }

        private Transaction GetRandomTransaction(int index = 1)
        {
            return new Transaction
            {
                Date = DateTime.Now.AddDays(index),
                Value = Random.Shared.Next(-20, 55),
                Name = names[Random.Shared.Next(names.Length)]
            };
        }
    }
}