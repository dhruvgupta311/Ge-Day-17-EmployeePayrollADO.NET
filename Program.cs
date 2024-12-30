using System;
using MySql.Data.MySqlClient;

namespace MySQLConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the connection string (ensure the correct username and password)
            string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD");

            // Check if the password was successfully retrieved
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Password not found in environment variables.");
                return;
            }

            // Correct way to include the password in the connection string
            string connectionString = $"Server=localhost; Database=payroll_service; User Id=root; Password={password};";

            try
            {
                // Create a new MySqlConnection object using the connection string
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Open the connection to the database
                    connection.Open();
                    Console.WriteLine("Connection Successful!");

                    // Example SQL query to fetch data from the employee_payroll table
                    string query = "SELECT id, name, gender, salary, start_date FROM employee_payroll"; // Specify columns explicitly

                    // Create a command object to execute the query
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Execute the query and use a reader to access the results
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if any data is returned
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Read each row and display the data
                                    Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}, Gender: {reader["gender"]}, Salary: {reader["salary"]}, Start Date: {reader["start_date"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found in the employee_payroll table.");
                            }
                        }
                    }
                }
            }
            catch (MySqlException mySqlEx)
            {
                // Handle MySQL-specific exceptions
                Console.WriteLine($"MySQL Error: {mySqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
