using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AzureAutocomplete.Data
{
    public class CosmosMongoDataAccess
    {
        private IMongoCollection<Address> _addresses;

        public CosmosMongoDataAccess(IConfiguration config)
        {
            var connectionString = config.GetValue<string>("CosmosMongoConnectionString");
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("StreetAddressDb");
            _addresses = database.GetCollection<Address>("StreetAddresses");
        }

        public string[] DoQuery(string SearchTerm)
        {
            var addressList = _addresses.Find(add => add.address.StartsWith(SearchTerm)).Limit(5).Project(add => add.address).ToList();

            return addressList.ToArray();
        }

        // Use this to load the data file into mongo since I couldn't find an easier tool
        public string LoadData()
        {
            string path = Environment.CurrentDirectory + "/Data/AddressData.txt";
            var lines = File.ReadAllLines(path);
            List<Address> adds = new List<Address>();

            // Insert every 500
            foreach (var line in lines)
            {
                adds.Add(new Address { address = line });
                if (adds.Count > 500)
                {
                    _addresses.InsertMany(adds);
                    adds.Clear();
                }
            }

            // add the remaining ones
            _addresses.InsertMany(adds);

            return "Loaded " + lines.Length + " rows.";            
        }

        internal sealed class Address
        {
            public string address { get; set; }
        }
    }
}
