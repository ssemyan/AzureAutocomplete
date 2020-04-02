using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AzureAutocomplete.Data
{
    public class SqlDataAccess
    {
        public static async Task<string[]> DoQuery(string SearchTerm, string connectionString)
        {
			List<string> addresses = new List<string>();
			using (var connection = new SqlConnection(connectionString))
			{
				var command = new SqlCommand("select top 5 Address from StreetAddresses where Address like @addressFiler", connection);
				command.Parameters.AddWithValue("@addressFiler", SearchTerm + "%");

				connection.Open();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						addresses.Add((string)reader["address"]);
					}
				}
			}
			return addresses.ToArray();
		}
    }
}
