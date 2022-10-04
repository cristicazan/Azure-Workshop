using Microsoft.AspNetCore.Mvc;

namespace Azure_Workshop.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        public BankAccountController() { }

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> GetTransactions()
        {
            return Enumerable.Range(1, 10).Select(index => new Transaction
            {
                Date = DateTime.Now.AddDays(index),
                Value = Random.Shared.Next(-20, 55),
                Name = names[Random.Shared.Next(names.Length)]
            }).ToArray();
        }
    }
}