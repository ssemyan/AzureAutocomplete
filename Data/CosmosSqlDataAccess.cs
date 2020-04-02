using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAutocomplete.Data
{
    public class CosmosSqlDataAccess
    {
        private Container _container;

        // Best to create as a singleton and reuse
        public CosmosSqlDataAccess(IConfiguration config)
        {
            var connectionString = config.GetValue<string>("CosmosSqlConnectionString");
            CosmosClient client = new CosmosClient(connectionString);
            _container = client.GetContainer("StreetAddressDb", "StreetAddress");
        }

        public async Task<string[]> DoQuery(string SearchTerm)
        {
            List<Address> addresses = new List<Address>();
            QueryDefinition query = new QueryDefinition("SELECT TOP 5 a.address FROM StreetAddress a WHERE STARTSWITH(a.address, @searchTerm)").WithParameter("@searchTerm", SearchTerm);
            FeedIterator<Address> resultSetIterator = _container.GetItemQueryIterator<Address>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<Address> response = await resultSetIterator.ReadNextAsync();
                addresses.AddRange(response);
            }

            return addresses.Select(a => a.address).ToArray();
        }

        internal sealed class Address
        {
            public string address { get; set; }
        }
    }
}
