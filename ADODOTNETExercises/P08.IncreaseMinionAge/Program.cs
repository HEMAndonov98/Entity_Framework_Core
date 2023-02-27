using System.Text;
using Microsoft.Data.SqlClient;

namespace P08.IncreaseMinionAge;
class Program
{
    static async Task Main(string[] args)
    {
        int[]? minionIds = Console.ReadLine()!
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();

        SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            using (connection)
            {
                for (int i = 0; i < minionIds.Length; i++)
                {
                    int? currentId = minionIds[i];

                    await UpdateMinionByIdAsync(currentId, connection, transaction);
                }

                string? output = await PrintAllMinions(connection, transaction);
                Console.WriteLine(output);
            }

           await transaction.RollbackAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private static async Task<string> PrintAllMinions(SqlConnection connection, SqlTransaction transaction)
    {
        SqlCommand command = new SqlCommand(Query.SelectAllMinionsCMD, connection);
        command.Transaction = transaction;
        SqlDataReader reader = await command.ExecuteReaderAsync();
        

        StringBuilder sb = new StringBuilder();

        using (reader)
        {
            while (await reader.ReadAsync())
            {
                string? minionName = reader["Name"].ToString();
                int minionAge = (int)reader["Age"];

                sb.AppendLine($"{minionName} {minionAge}");
            }
        }

        return sb.ToString().Trim();
    }

    private static async Task UpdateMinionByIdAsync(int? id, SqlConnection connection, SqlTransaction transaction)
    {
        SqlCommand command = new SqlCommand(Query.UpdateNameAndAgeOfMinionCMD, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Transaction = transaction;

        await command.ExecuteNonQueryAsync();
    }
}

