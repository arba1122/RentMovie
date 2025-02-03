using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        // Skapa en anslutning till SQL Server
        using IDbConnection connection = new SqlConnection(
            "Server=gondolin667.org;" +
            "Database=yhstudent60;" +
            "User ID=yhstudent60;" +
            "Password=6BB^jxm5ssFO;" +
            "Encrypt=False;" +
            "TrustServerCertificate=False;"
        );

        try
        {
            connection.Open();
            Console.WriteLine("Ansluten till databasen!");

            // Skapa en SQL-fråga
            string query = "SELECT * FROM Customer";

            // Skapa ett kommando
            using IDbCommand command = connection.CreateCommand();
            command.CommandText = query;

            // Exekvera kommandot och hämta resultat
            using IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]}, Namn: {reader["Name"]}, Email: {reader["Email"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fel vid anslutning: " + ex.Message);
        }
    }
}
