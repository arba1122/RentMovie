using System;                           // Inkluderar grundläggande systemfunktioner
using System.Collections.Generic;       // Inkluderar generiska samlingar (behövs ej direkt här)
using System.Data;                      // Inkluderar gränssnitt för databasåtkomst (IDbConnection)
using System.Data.SqlClient;            // Inkluderar SQL Server-specifika anslutningsklasser
using System.IO;                        // Inkluderar filhantering (nödvändigt för File.ReadAllText)
using Dapper;                           // Inkluderar Dapper, ett mikroramverk för databasanrop

public class DataAccess
{
    // Fält för att lagra anslutningssträngen
    private string connectionString;

    // Konstruktor: anropas när ett DataAccess-objekt skapas
    public DataAccess()
    {
        // Hämtar anslutningssträngen från filen "connectionstring.txt"
        // och sparar den i fältet connectionString
        connectionString = GetConnectionString();
    }

    // Metod för att skapa och returnera en databasanslutning
    public IDbConnection GetConnection()
    {
        // Skapar ett nytt SqlConnection-objekt med den sparade anslutningssträngen
        // och returnerar det som en IDbConnection
        return new SqlConnection(connectionString);
    }

    // Privat metod för att läsa in anslutningssträngen från en textfil
    private string GetConnectionString()
    {
        // Läser in hela innehållet från filen "connectionstring.txt" och returnerar det
        return File.ReadAllText("connectionstring.txt");
    }
}
