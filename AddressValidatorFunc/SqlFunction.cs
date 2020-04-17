using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AddressValidatorFunc
{
    public static class SqlFunction
    {
        [FunctionName("GetAddressesSql")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AddressValidator/sql/{searchTerm}")] HttpRequest req, string searchTerm, ILogger log)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new BadRequestObjectResult("Query parameter 'add' required.");
            }

            log.LogInformation("Processing request for search term: " + searchTerm);

            List<string> addresses = new List<string>();

            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                var command = new SqlCommand("select top 5 Address from StreetAddresses where Address like @addressFiler", conn);
                command.Parameters.AddWithValue("@addressFiler", searchTerm + "%");
                conn.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        addresses.Add((string)reader["address"]);
                    }
                }
            }

            return new OkObjectResult(addresses.ToArray());
        }
    }
}
