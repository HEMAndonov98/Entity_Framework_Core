using Microsoft.Data.SqlClient;

namespace P01.InitialSetup;
class Program
{
    static void Main(string[] args)
    {
        SqlConnection connection = new SqlConnection("Put your connection string here");
        connection.Open();
        using (connection)
        {
        }
        connection.Close();
    }
}

