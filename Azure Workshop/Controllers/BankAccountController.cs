using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TelemetryClient _telemetryClient;
        private readonly BankDbContext _bankDbContext;

        public BankAccountController(ServiceBusClient serviceBusClient, IConfiguration configuration, TelemetryClient telemetryClient, BankDbContext bankDbContext)
        {
            _serviceBusClient = serviceBusClient;
            _blobContainerClient = new BlobContainerClient(configuration.GetConnectionString("StorageAccount"), "transactions");
            _telemetryClient = telemetryClient;
            _bankDbContext = bankDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            _telemetryClient.TrackTrace("Hello from here");

            var transactions = await _bankDbContext.Transactions
                .Select(t => new Transaction { Date = t.Date, Name = t.Name, Value = t.Value })
                .ToArrayAsync();

            return transactions;
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