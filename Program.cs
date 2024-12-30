using System;
using System.Collections.Generic;
using System.Data.Odbc; // ODBC Namespace
using System.Configuration;

public class EmployeePayroll
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; } // New field

    // Constructor for backward compatibility
    public EmployeePayroll(int id, string name, string gender, decimal salary, DateTime startDate)
    {
        Id = id;
        Name = name;
        Gender = gender;
        Salary = salary;
        StartDate = startDate;
    }

    // Constructor without StartDate for backward compatibility
    public EmployeePayroll(int id, string name, string gender, decimal salary)
        : this(id, name, gender, salary, DateTime.MinValue) // Default to DateTime.MinValue
    { }
}

namespace EmployeePayrollService
{
    class Program
    {
        static void Main(string[] args)
        {
            List<EmployeePayroll> payrollList = GetEmployeePayrollData();
            foreach (var payroll in payrollList)
            {
                Console.WriteLine($"ID: {payroll.Id}, Name: {payroll.Name}, Gender: {payroll.Gender}, Salary: {payroll.Salary}, Start Date: {payroll.StartDate.ToShortDateString()}");
            }
        }

        // Method to retrieve employee payroll data from database
        public static List<EmployeePayroll> GetEmployeePayrollData()
        {
            List<EmployeePayroll> payrollList = new List<EmployeePayroll>();
            string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD");

            // Define your connection string (can be stored in App.config or use environment variable)
            string connectionString = $"Driver={{MySQL ODBC 9.1 ANSI Driver}};Server=Dhruv;Database=payroll_service;User=root;Password={password};";

            try
            {
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    // SQL Query to get employee payroll data
                    string query = "SELECT id, name, gender, salary, start_date FROM employee_payroll";

                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0); // Column 0 is the ID (integer)
                        string name = reader.GetString(1); // Column 1 is the Name (string)
                        string gender = reader.GetString(2); // Column 2 is the Gender (string)
                        decimal salary = reader.GetDecimal(3); // Column 3 is the Salary (decimal)
                        DateTime startDate = reader.GetDateTime(4); // Column 4 is the Start Date (DateTime)

                        // Create and add the EmployeePayroll object to the list
                        EmployeePayroll payroll = new EmployeePayroll(id, name, gender, salary, startDate);
                        payrollList.Add(payroll);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return payrollList;
        }
    }
}
