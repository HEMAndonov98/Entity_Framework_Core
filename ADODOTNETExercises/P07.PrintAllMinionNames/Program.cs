using Microsoft.Data.SqlClient;

namespace P07.PrintAllMinionNames;
class Program
{
    static async Task Main(string[] args)
    {
        List<string?> minions = new List<string?>();
        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();

        using (connection)
        {
            SqlCommand command = new SqlCommand(Query.SelectAllMinions, connection);
            SqlDataReader reader;

            reader = await command.ExecuteReaderAsync();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    minions.Add(reader["Name"].ToString());
                }
            }

            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[i]);

                int currentLastMinionIndex = minions.Count - 1 - i;

                if (currentLastMinionIndex > i)
                {
                    Console.WriteLine(minions[currentLastMinionIndex]);
                }
            }
        }
    }
}

