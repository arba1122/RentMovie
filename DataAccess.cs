using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

public class DataAccess
{
    // Anslutningssträng till databasen
    private static string connectionString = "Server=gondolin667.org;" +
                                             "Database=yhstudent60_movierental;" +
                                             "User ID=yhstudent60;" +
                                             "Password=6BB^jxm5ssFO;" +
                                             "Encrypt=False;" +
                                             "TrustServerCertificate=True;";

    // Skapar och returnerar en databasanslutning
    public IDbConnection GetConnection()
    {
        return new Microsoft.Data.SqlClient.SqlConnection(connectionString);
    }

    // Hämtar alla filmer från databasen
    public IEnumerable<Movie> GetAllMovies()
    {
        using var connection = GetConnection();
        return connection.Query<Movie>("SELECT * FROM Movie");
    }

    // Hämtar en kund baserat på ID
    public Customer GetCustomerById(int id)
    {
        using var connection = GetConnection();
        var customer = connection.QueryFirstOrDefault<Customer>(
            "SELECT * FROM Customer WHERE CustomerId = @Id",
            new { Id = id }
        );

        if (customer == null)
        {
            Console.WriteLine($"Ingen kund hittades med ID {id}.");
            return new Customer(); // Returnerar en tom kund
        }

        return customer;
    }

    // Lägger till en ny film i databasen
    public void AddMovie(Movie movie)
    {
        using var connection = GetConnection();
        connection.Execute(
            "INSERT INTO Movie (Title, Genre, ReleaseDate, RentalPrice) VALUES (@Title, @Genre, @ReleaseDate, @RentalPrice)",
            movie
        );
    }

    // Tar bort en film från databasen baserat på film-ID
    public void RemoveMovie(int movieId)
    {
        using var connection = GetConnection();
        connection.Execute("DELETE FROM Movie WHERE MovieID = @MovieID", new { MovieID = movieId });
    }

    // Hämtar de mest uthyrda filmerna med en JOIN mellan Rental och Movie
    public IEnumerable<(Movie, int)> GetMostRentedMovies()
    {
        using var connection = GetConnection();
        var rentedMovies = connection.Query<(int MovieID, string Title, int RentCount)>(
            @"SELECT m.MovieID, m.Title, COUNT(r.MovieId) AS RentCount
              FROM Rental r
              JOIN Movie m ON r.MovieId = m.MovieID  -- Kopplar Rental till Movie med en JOIN
              GROUP BY m.MovieID, m.Title
              ORDER BY RentCount DESC;"
        );

        return rentedMovies.Select(r => (new Movie { MovieID = r.MovieID, Title = r.Title }, r.RentCount));
    }

    // Hämtar alla försenade uthyrningar med en JOIN mellan Rental, Customer och Movie
    public IEnumerable<OverdueRental> GetOverdueRentals()
    {
        using var connection = GetConnection();
        return connection.Query<OverdueRental>(
            @"SELECT r.RentalID, c.Name AS CustomerName, m.Title AS MovieTitle, r.ReturnDate
              FROM Rental r
              JOIN Customer c ON r.CustomerID = c.CustomerID  -- Kopplar Rental till Customer
              JOIN Movie m ON r.MovieID = m.MovieID  -- Kopplar Rental till Movie
              WHERE r.ReturnDate < GETDATE();  -- Filtrerar ut endast försenade uthyrningar"
        );
    }

    // Hämtar en film baserat på titel
    public Movie GetMovieByTitle(string title)
    {
        using var connection = GetConnection();
        return connection.QueryFirstOrDefault<Movie>("SELECT * FROM Movie WHERE Title = @Title", new { Title = title });
    }

    // Räknar ut genomsnittligt antal uthyrda filmer per dag genom att gruppera per datum
    public double GetAverageMoviesPerDay()
    {
        using var connection = GetConnection();
        return connection.QueryFirstOrDefault<double>(
            @"SELECT AVG(MoviesPerDay) AS AverageMoviesPerDay
              FROM (
                  SELECT RentalDate, COUNT(*) AS MoviesPerDay
                  FROM Rental
                  GROUP BY RentalDate
              ) AS DailyRentals;"
        );
    }


    // Hämtar uthyrningshistorik för en kund och räknar hur många filmer kunden hyrt
    public IEnumerable<(string CustomerName, string MovieTitle, string Category, int TotalRentals)> GetCustomerRentalHistory(int customerId)
    {
        using var connection = GetConnection();
        var result = connection.Query<(string CustomerName, string MovieTitle, string Category, int TotalRentals)>(
            @"SELECT c.Name AS CustomerName, m.Title AS MovieTitle, cat.CatName AS Category, COUNT(r.Id) AS TotalRentals
          FROM Rental r
          JOIN Customer c ON r.CustomerId = c.CustomerId  -- Kopplar Rental till Customer
          JOIN Movie m ON r.MovieId = m.MovieID  -- Kopplar Rental till Movie
          JOIN Category cat ON m.CategoryId = cat.Id  -- Kopplar Movie till Category
          WHERE c.CustomerId = @CustomerId
          GROUP BY c.Name, m.Title, cat.CatName
          ORDER BY TotalRentals DESC;",
            new { CustomerId = customerId }
        );

        return result;
    }

}
