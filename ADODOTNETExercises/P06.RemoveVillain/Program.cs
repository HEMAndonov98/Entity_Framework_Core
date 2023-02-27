using Microsoft.Data.SqlClient;

namespace P06.RemoveVillain;
class Program
{
    static async Task Main(string[] args)
    {
        int? villainid = int.Parse(Console.ReadLine()!);

        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();

        using (connection)
        {
            object? villainObj = await GetVillainByIdAsync(villainid, connection);

            if (villainObj != null)
            {
                string villainName = (string)villainObj;
                int numberOfDeletedMinions = await ReleaseVillainMinions(villainid, connection);
                await DeleteVillainAsync(villainid, connection);

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{numberOfDeletedMinions} minions were released.");
            }
            else
            {
                Console.WriteLine("No such villain was found.");
            }
        }
    }

    private static Task<int> ReleaseVillainMinions(int? villainid, SqlConnection connection)
    {
        SqlCommand command = new SqlCommand(Query.ReleaseVillanMinionsCMD, connection);
        command.Parameters.AddWithValue("@villainId", villainid);

        return command.ExecuteNonQueryAsync();
    }

    private static async Task DeleteVillainAsync(int? villainid, SqlConnection connection)
    {
        SqlCommand command = new SqlCommand(Query.DeleteVillainByIdCMD, connection);
        command.Parameters.AddWithValue("@villainId", villainid);

        await command.ExecuteNonQueryAsync();
    }

    private static Task<object?> GetVillainByIdAsync(int? villainid, SqlConnection connection)
    {
        SqlCommand command = new SqlCommand(Query.FindVillainByIdCMD, connection);
        command.Parameters.AddWithValue("@villainId", villainid);

        return command.ExecuteScalarAsync();
    }
}

