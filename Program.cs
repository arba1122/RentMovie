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
           
        }