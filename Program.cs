using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;




public class Program
{
    public static void Main()
    {
        Menu menu = new Menu();
        menu.ShowMenu();
    }
}
