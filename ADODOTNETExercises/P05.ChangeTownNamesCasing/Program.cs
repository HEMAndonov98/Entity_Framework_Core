using System.Text;
using Microsoft.Data.SqlClient;

namespace P05.ChangeTownNamesCasing;
class Program
{
    static async Task Main(string[] args)
    {
        string? countryName = Console.ReadLine();

        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();
        using (connection)
        {
            int affectedRowsCount = 0;
            affectedRowsCount = await UpdateCountryNameAsync(connection, countryName);

            if (affectedRowsCount > 0)
            {
                Console.WriteLine($"{affectedRowsCount} town names were affected.");
                string affectedTowns = await PrintAffectedTownsAsync(connection, countryName);
                Console.WriteLine($"[ {affectedTowns} ]");
            }
            else
            {
                Console.WriteLine("No town names were affected.");
            }

        }
    }

    private static async Task<string> PrintAffectedTownsAsync(SqlConnection connection, string? countryName)
    {
        SqlCommand command = new SqlCommand(Query.PrintAffectedTownsCMD, connection);
        command.Parameters.AddWithValue("@countryName", countryName);
        SqlDataReader reader = await command.ExecuteReaderAsync();
        List<string?> towns = new List<string?>();

        using (reader)
        {
            while (await reader.ReadAsync())
            {
                towns.Add(reader["Name"].ToString());
            }
        }

        return string.Join(", ", towns);
    }

    private static async Task<int> UpdateCountryNameAsync(SqlConnection connection, string? countryName)
    {
        SqlCommand command = new SqlCommand(Query.UpdateCountryNamesToUpperCMD, connection);
        command.Parameters.AddWithValue("@countryName", countryName);

        return await command.ExecuteNonQueryAsync();
    }
}

