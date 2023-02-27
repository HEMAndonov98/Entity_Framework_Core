using System.Data;
using Microsoft.Data.SqlClient;

namespace P09.IncreaseMinionAgeSP;
class Program
{
    static async Task Main(string[] args)
    {
        int? minionId = int.Parse(Console.ReadLine()!);

        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();

        using (connection)
        {
            string proc = "usp_GetOlder";

            SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            SqlCommand command = new SqlCommand(proc, connection, transaction)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.Add("@Id", SqlDbType.Int).Value = minionId;


            try
            {
                await command.ExecuteNonQueryAsync();

                command = new SqlCommand(Query.SelectMinions, connection, transaction);
                command.Parameters.AddWithValue("@Id", minionId);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        string? minionName = reader["Name"].ToString();
                        int minionAge = (int)reader["Age"];

                        Console.WriteLine($"{minionName} – {minionAge} years old");
                    }
                }

                await transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

