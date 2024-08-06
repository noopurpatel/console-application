using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main(string[] args)
    {

        string connectionString = "Data Source=Axzyte385;Initial Catalog=AdventureWorksLT2017;Trusted_Connection=True;";

        // Call a method to display the main menu and handle user input
        ShowMainMenu(connectionString);
    }

    static void ShowMainMenu(string connectionString)
    {
        while (true)
        {
            Console.WriteLine("1 - Fetch first 10 Products");
            Console.WriteLine("2 - Fetch first 20 Customers and Addresses");
            Console.WriteLine("3 - Calculate total cost of all Products");
            Console.WriteLine("4 - Fetch first 10 Products (Disconnected)");
            Console.WriteLine("5 - Fetch first 20 Customers and Addresses (Disconnected)");
            Console.WriteLine("6 - Calculate total cost of all Products (Disconnected)");
            Console.WriteLine("0 - Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    FetchFirst10Products(connectionString);
                    break;
                case "2":
                    FetchFirst20CustomersAndAddresses(connectionString);
                    break;
                case "3":
                    CalculateTotalCost(connectionString);
                    break;
                case "4":
                    FetchFirst10ProductsDisconnected(connectionString);
                    break;
                case "5":
                    FetchFirst20CustomersAndAddressesDisconnected(connectionString);
                    break;
                case "6":
                    CalculateTotalCostDisconnected(connectionString);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    static void FetchFirst10Products(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT TOP 10 ProductID, Name, ProductNumber, Color, StandardCost FROM SalesLT.Product WHERE Name LIKE 'H%' OR Name LIKE 'M%' OR Name LIKE 'L%'";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("ProductID | Name       | ProductNumber | Color | StandardCost");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["ProductID"],-10} | {reader["Name"],-10} | {reader["ProductNumber"],-14} | {reader["Color"],-5} | {reader["StandardCost"],-12}");
            }
        }
    }

    static void FetchFirst20CustomersAndAddresses(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT TOP 20 C.FirstName, CA.AddressType 
                FROM SalesLT.Customer C
                JOIN SalesLT.CustomerAddress CA ON C.CustomerID = CA.CustomerID";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("FirstName | AddressType");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["FirstName"],-10} | {reader["AddressType"],-12}");
            }
        }
    }

    static void CalculateTotalCost(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT SUM(StandardCost) AS TotalCost FROM SalesLT.Product";
            SqlCommand command = new SqlCommand(query, connection);
            object result = command.ExecuteScalar();

            Console.WriteLine($"Total Cost of All Products: {result}");
        }
    }

    static void FetchFirst10ProductsDisconnected(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT TOP 10 ProductID, Name, ProductNumber, Color, StandardCost FROM SalesLT.Product WHERE Name LIKE 'H%' OR Name LIKE 'M%' OR Name LIKE 'L%'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Products");

            Console.WriteLine("ProductID | Name       | ProductNumber | Color | StandardCost");
            foreach (DataRow row in dataSet.Tables["Products"].Rows)
            {
                Console.WriteLine($"{row["ProductID"],-10} | {row["Name"],-10} | {row["ProductNumber"],-14} | {row["Color"],-5} | {row["StandardCost"],-12}");
            }
        }
    }

    static void FetchFirst20CustomersAndAddressesDisconnected(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT TOP 20 C.FirstName, CA.AddressType 
                FROM SalesLT.Customer C
                JOIN SalesLT.CustomerAddress CA ON C.CustomerID = CA.CustomerID";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CustomerAddresses");

            Console.WriteLine("FirstName | AddressType");
            foreach (DataRow row in dataSet.Tables["CustomerAddresses"].Rows)
            {
                Console.WriteLine($"{row["FirstName"],-10} | {row["AddressType"],-12}");
            }
        }
    }

    static void CalculateTotalCostDisconnected(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT StandardCost FROM SalesLT.Product";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Products");

            decimal totalCost = 0;
            foreach (DataRow row in dataSet.Tables["Products"].Rows)
            {
                totalCost += (decimal)row["StandardCost"];
            }

            Console.WriteLine($"Total Cost of All Products: {totalCost}");
        }
    }
}
