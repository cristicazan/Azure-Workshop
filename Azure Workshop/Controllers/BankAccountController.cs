using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
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
        private readonly BlobContainerClient _blobContainerClient;

        public BankAccountController(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            _serviceBusClient = serviceBusClient;
            _blobContainerClient = new BlobContainerClient(configuration.GetConnectionString("StorageAccount"), "transactions");
        }

        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            return Enumerable.Range(1, 10).Select(GetRandomTransaction).ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> UploadTransactions(IFormFile file)
        {
            BlobClient blob = _blobContainerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageTransaction()
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(GetRandomTransaction()));

            var sender = _serviceBusClient.CreateSender("transactions");

            await sender.SendMessageAsync(message);

            return Ok();
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