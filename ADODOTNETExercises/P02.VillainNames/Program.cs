using Microsoft.Data.SqlClient;

namespace P02.VillainNames;
class Program
{
    static void Main(string[] args)
    {
        SqlConnection connection = new SqlConnection(Config.ConnectionString);
        connection.Open();
        using (connection)
        {
            SqlCommand command = new SqlCommand("  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  " +
                "\n    FROM Villains AS v " +
                "\n    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId \nGROUP BY v.Id, v.Name " +
                "\n  HAVING COUNT(mv.VillainId) > 3 \nORDER BY COUNT(mv.VillainId)", connection);
            SqlDataReader dataReader = command.ExecuteReader();

            using (dataReader)
            {
                while (dataReader.Read())
                {
                    string villainName = dataReader["Name"].ToString();
                    string minionsNumber = dataReader["MinionsCount"].ToString();

                    Console.WriteLine($"{villainName} - {minionsNumber}");
                }
            }
        }
    }
}

