using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Cosmos;
using System.Linq;

namespace AddressValidatorFunc
{
    public class CosmosDbFunction
    {
        private readonly Container _container;
        public CosmosDbFunction(CosmosClient cosmosClient)
        {
            _container = cosmosClient.GetContainer("StreetAddressDb", "StreetAddress");
        }

        [FunctionName("CosmosDbFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AddressValidator/csql/{searchTerm}")] HttpRequest req,
            string searchTerm, ILogger log)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new NotFoundObjectResult("Query parameter 'add' required.");
            }

            log.LogInformation("Processing request for search term: " + searchTerm);

            List<Address> addresses = new List<Address>();
            QueryDefinition query = new QueryDefinition("SELECT TOP 5 a.address FROM StreetAddress a WHERE STARTSWITH(a.address, @searchTerm)").WithParameter("@searchTerm", searchTerm);
            FeedIterator<Address> resultSetIterator = _container.GetItemQueryIterator<Address>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<Address> response = await resultSetIterator.ReadNextAsync();
                addresses.AddRange(response);
            }

            return new OkObjectResult(addresses.Select(a => a.address).ToArray());
        }

        public class Address
        {
            public string address { get; set; }
        }
    }
}
