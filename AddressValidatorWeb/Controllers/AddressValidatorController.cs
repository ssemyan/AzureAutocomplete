using System;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using AzureAutocomplete.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AzureAutocomplete.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressValidatorController : ControllerBase
    {
        private IConfiguration _configuration;
        private CosmosSqlDataAccess _cosmosSqlData;
        private CosmosMongoDataAccess _cosmosMongoData;

        public AddressValidatorController(IConfiguration iconfig, CosmosSqlDataAccess cosmosSqlData, CosmosMongoDataAccess cosmosMongoData)
        {
            _configuration = iconfig;
            _cosmosSqlData = cosmosSqlData;
            _cosmosMongoData = cosmosMongoData;
        }

        [HttpGet("{dataStore}/{searchTerm}")]
        public async Task<IEnumerable> Get(string dataStore, string searchTerm)
        {
            string term = WebUtility.UrlDecode(searchTerm);

            if (dataStore == "sql")
            {
                return await SqlDataAccess.DoQuery(term, _configuration.GetValue<string>("SqlConnectionString"));
            }
            else if (dataStore == "csql")
            {
                return await _cosmosSqlData.DoQuery(term);
            }
            else if (dataStore == "cmongo")
            {
                return _cosmosMongoData.DoQuery(term);
            }
            else
            {
                throw new Exception("Unknown data store type");
            }
        }

        // Insert data into datastore
        [HttpGet("{dataStore}")]
        public string Get(string dataStore)
        {
            // api/AddressValidator/cmongo
            if (dataStore == "cmongo")
            {
                return _cosmosMongoData.LoadData();
            }
            else
            {
                throw new Exception("Unknown data store type");
            }
        }
    }
}