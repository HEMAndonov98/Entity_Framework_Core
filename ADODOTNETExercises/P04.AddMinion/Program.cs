using Microsoft.Data.SqlClient;

namespace P04.AddMinion;
class Program
{
    static async Task Main(string[] args)
    {

        string[] minionArgs = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string minionName = minionArgs[1];
        int minionAge = int.Parse(minionArgs[2]);
        string townName = minionArgs[3];

        string[] villainArgs = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string villainName = villainArgs[1];

        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        await connection.OpenAsync();
        await using (connection)
        {
            object? townObj = await GetTownByNameAsync(townName, connection);

            if (townObj == null)
            {
                await AddTown(townName, connection);
                townObj = await GetTownByNameAsync(townName, connection);
            }

            object? villainObj = await GetVillainByNameAsync(villainName, connection);

            if (villainObj == null)
            {
                await AddVillain(villainName, connection);
                villainObj = await GetVillainByNameAsync(villainName, connection);
            }

            object? minionObj = await GetMinionByNameAsync(minionName, connection);

            if (minionObj == null)
            {
                    await AddMinion(minionName, minionAge, (int)townObj!, connection);
                    minionObj = GetMinionByNameAsync(minionName, connection);
            }

            try
            {
                await SetMinionMaster((int)minionObj, (int)villainObj!, connection);
                //Replace with writer
                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

    private static async Task<object?> GetMinionByNameAsync(string minionName, SqlConnection connection)
    {
        SqlCommand command = new SqlCommand(Query.FindMinionByName, connection);
        command.Parameters.AddWithValue("@Name", minionName);

        return await command.ExecuteScalarAsync();
    }

    private static async Task<object?> GetVillainByNameAsync(string villainName, SqlConnection connection)
    {
        SqlCommand sqlCommand = new SqlCommand(Query.FindVillainByNameQuery, connection);
        sqlCommand.Parameters.AddWithValue("@Name", villainName);

        return await sqlCommand.ExecuteScalarAsync();
    }

    private static async Task<object> GetTownByNameAsync(string townName, SqlConnection connection)
    {
        object? townObj = null;
        SqlCommand command = new SqlCommand(Query.FindTownByNameQuery, connection);
        command.Parameters.AddWithValue("@townName", townName);
        townObj = await command.ExecuteScalarAsync();
        return townObj!;
    }

    private static async Task AddTown(string townName, SqlConnection connection)
    {
        SqlTransaction transaction;
        transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        SqlCommand command = new SqlCommand(Query.AddTownToDatabase, connection, transaction);
        command.Parameters.AddWithValue("@townName", townName);


        using (transaction)
        {
            try
            {
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                //Replace with writer
                Console.WriteLine($"Town {townName} was added to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit Exception: {ex.GetType()}");
                Console.WriteLine($"   Message: {ex.Message}");


                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Rollback Exception: {ex2.GetType()}");
                    Console.WriteLine($"     Message: {ex2.Message}");
                }
            }
        }

    }

    private static async Task AddMinion(string minionName, int minionAge, int townId, SqlConnection connection)
    {
        SqlTransaction transaction;
        transaction = (SqlTransaction)await connection.BeginTransactionAsync();


        SqlCommand command = new SqlCommand(Query.AddMinionWithValues, connection, transaction);
        command.Parameters.AddWithValue("@name", minionName);
        command.Parameters.AddWithValue("@age", minionAge);
        command.Parameters.AddWithValue("@townId", townId);


        using (transaction)
        {

            try
            {
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit Exception: {ex.GetType()}");
                Console.WriteLine($"   Message: {ex.Message}");

                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Rollback Exception: {ex2.GetType()}");
                    Console.WriteLine($"     Message: {ex2.Message}");
                }
            }
        }
    }

    private static async Task AddVillain(string villainName, SqlConnection connection)
    {
        SqlTransaction transaction;
        transaction = (SqlTransaction)await connection.BeginTransactionAsync();


        SqlCommand command = new SqlCommand(Query.AddVillainWithDefaultEvilness, connection, transaction);
        command.Parameters.AddWithValue("@villainName", villainName);

        using (transaction)
        {
            try
            {
                await command.ExecuteNonQueryAsync();

                //Replace with writer
                Console.WriteLine($"Villain {villainName} was added to the database.");

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit Exception: {ex.GetType()}");
                Console.WriteLine($"   Message: {ex.Message}");

                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Rollback Exception: {ex2.GetType()}");
                    Console.WriteLine($"     Message: {ex2.Message}");
                }
            }
        }
    }

    private static async Task SetMinionMaster(int minionId, int villainId, SqlConnection connection)
    {
        SqlTransaction transaction;
        transaction = (SqlTransaction)await connection.BeginTransactionAsync();


        SqlCommand command = new SqlCommand(Query.AddNewMinionMaster, connection, transaction);
        command.Parameters.AddWithValue("@minionId", minionId);
        command.Parameters.AddWithValue("@villainId", villainId);


        using (transaction)
        {
            try
            {
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit Exception: {ex.GetType()}");
                Console.WriteLine($"   Message: {ex.Message}");

                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Rollback Exception: {ex2.GetType()}");
                    Console.WriteLine($"     Message: {ex2.Message}");
                }
            }
        }
    }

}

