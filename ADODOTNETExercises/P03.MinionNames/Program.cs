using Microsoft.Data.SqlClient;

namespace P03.MinionNames;
class Program
{

    public const string InvalidIdError = "No villain with ID {0} exists in the database";


    static void Main(string[] args)
    {


        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        connection.Open();
        using (connection)
        {
            string? id = Console.ReadLine();

            string villainName = GetVilainName(id!, connection);

            Console.WriteLine(villainName);

            PrinMinions(connection, id);
        }
    }

    private static void PrinMinions(SqlConnection connection, string? id)
    {
        bool isRead = false;

        SqlCommand command = new SqlCommand("\n" +
            "SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,\n" +
            " m.Name, \n" +
            " m.Age\n" +
            "FROM MinionsVillains AS mv\n" +
            "JOIN Minions As m ON mv.MinionId = m.Id\n" +
            "WHERE mv.VillainId = @Id\n" +
            "ORDER BY m.Name", connection);

        command.Parameters.AddWithValue("@Id", int.Parse(id));
        SqlDataReader reader = command.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                isRead = true;
                int minionCount = 1;
                Console.WriteLine($"{minionCount++}. {reader["Name"]} {reader["Age"]}");
            }

            if (isRead == false)
            {
                Console.Write("(no minions)");
            }
        }

    }

    private static string GetVilainName(string id, SqlConnection sqlConnection)
    {
        string name = string.Empty;
        bool IsFound = false;

        if (string.IsNullOrEmpty(id))
        {
            return string.Format(InvalidIdError, id);
        }

        SqlCommand command = new SqlCommand("SELECT Name FROM Villains WHERE Id = @Id", sqlConnection);

        command.Parameters.AddWithValue("@Id", id);

        SqlDataReader reader = command.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                name = $"Villain: {reader["Name"]}";
                IsFound = true;
            }

            if (IsFound)
            {
                return name;
            }

            return string.Format(InvalidIdError, id);
        }
    }
}

