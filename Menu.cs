public class Menu
{
    // Skapar en instans av DataAccess 
    private DataAccess dataAccess = new DataAccess();

    // Visar huvudmenyn och hanterar användarens val
    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== FILMHYRNINGS-SYSTEM ===");
            Console.WriteLine("1. Visa alla filmer");
            Console.WriteLine("2. Hämta kundinfo");
            Console.WriteLine("3. Lägg till ny film");
            Console.WriteLine("4. Ta bort film");
            Console.WriteLine("5. Mest uthyrda filmer");
            Console.WriteLine("6. Lista försenade uthyrningar");
            Console.WriteLine("7. Visa genomsnittligt antal uthyrda filmer per dag");
            Console.WriteLine("8. Visa kundens uthyrningshistorik");
            Console.WriteLine("9. Avsluta");
            Console.Write("Välj ett alternativ: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    DisplayAllMovies();
                    break;
                case "2":
                    GetCustomerInfo();
                    break;
                case "3":
                    AddNewMovie();
                    break;
                case "4":
                    RemoveMovieMenu();
                    break;
                case "5":
                    MostRentedMovies();
                    break;
                case "6":
                    ListOverdueRentals();
                    break;
                case "7":
                    ShowAverageMoviesPerDay();
                    break;
                case "8":
                    ShowCustomerRentalHistory();
                    break;

                case "9":
                    Console.WriteLine("Avslutar programmet...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt alternativ, försök igen.");
                    break;
            }
        }
    }

    // Visar alla filmer som finns i databasen
    private void DisplayAllMovies()
    {
        var movies = dataAccess.GetAllMovies();
        foreach (var movie in movies)
        {
            Console.WriteLine($"{movie.MovieID}: {movie.Title} ({movie.Genre})");
        }
    }

    // Hämtar och visar information om en kund baserat på ID
    private void GetCustomerInfo()
    {
        Console.WriteLine("Ange kund-ID: ");
        if (int.TryParse(Console.ReadLine(), out int customerId))
        {
            var customer = dataAccess.GetCustomerById(customerId);
            Console.WriteLine($"Namn: {customer.Name}, Kontaktinfo: {customer.ContactInfo}, Medlemskap: {customer.MembershipLevel}");
        }
        else
        {
            Console.WriteLine("Ogiltigt kund-ID.");
        }
    }

    // Lägger till en ny film i databasen
    private void AddNewMovie()
    {
        Console.WriteLine("Ange titel: ");
        string title = Console.ReadLine();

        Console.WriteLine("Ange genre: ");
        string genre = Console.ReadLine();

        Console.WriteLine("Ange releaseår: ");
        int releaseYear = int.Parse(Console.ReadLine());

        DateTime releaseDate = new DateTime(releaseYear, 1, 1); // Skapar ett datum med 1 januari som standard
        Console.Write("Ange hyrpris: ");
        decimal rentalPrice = decimal.Parse(Console.ReadLine());

        Movie movie = new Movie { Title = title, Genre = genre, ReleaseDate = releaseDate, RentalPrice = rentalPrice };
        dataAccess.AddMovie(movie);

        Console.WriteLine("Film tillagd!");
    }

    // Tar bort en film från databasen baserat på titel
    private void RemoveMovieMenu()
    {
        Console.WriteLine("Ange filmens titel: ");
        string title = Console.ReadLine();

        var movie = dataAccess.GetMovieByTitle(title);

        if (movie == null)
        {
            Console.WriteLine($"Ingen film med titeln {title} hittades.");
            return;
        }

        Console.WriteLine($"Är du säker på att du vill ta bort \"{movie.Title}\"? (ja/nej): ");
        string confirmation = Console.ReadLine();

        if (confirmation == "ja")
        {
            dataAccess.RemoveMovie(movie.MovieID);
            Console.WriteLine("Filmen har tagits bort!");
        }
        else
        {
            Console.WriteLine("Borttagning avbruten.");
        }
    }

    // Visar de mest uthyrda filmerna i databasen
    private void MostRentedMovies()
    {
        var movies = dataAccess.GetMostRentedMovies();
        Console.WriteLine("\nMEST UTHYRDA FILMER:");
        foreach (var (movie, count) in movies)
        {
            Console.WriteLine($"{movie.Title} - Uthyrd {count} gånger");
        }
    }

    // Visar en lista över alla försenade uthyrningar
    private void ListOverdueRentals()
    {
        var rentals = dataAccess.GetOverdueRentals();
        Console.WriteLine("FÖRSENADE UTHYRNINGAR");

        foreach (var rental in rentals)
        {
            Console.WriteLine($"Uthyrning ID: {rental.RentalID}, Kund: {rental.CustomerName}, Film: {rental.MovieTitle}, Förfallodatum: {rental.ReturnDate}");
        }
    }

    // Visar genomsnittligt antal uthyrningar per dag
    private void ShowAverageMoviesPerDay()
    {
        double averageMovies = dataAccess.GetAverageMoviesPerDay();
        Console.WriteLine($"I genomsnitt hyrs {averageMovies:F2} filmer per dag.");
    }


    // Visar en kunds uthyrningshistorik
    private void ShowCustomerRentalHistory()
    {
        Console.Write("Ange kundens ID: ");
        int customerId = int.Parse(Console.ReadLine());

        var rentals = dataAccess.GetCustomerRentalHistory(customerId);

        Console.WriteLine("UTHYRNINGS-HISTORIK:");
        foreach (var rental in rentals)
        {
            Console.WriteLine($"Film: {rental.MovieTitle}, Hyrd: {rental.TotalRentals} gånger");
        }
    }

}



