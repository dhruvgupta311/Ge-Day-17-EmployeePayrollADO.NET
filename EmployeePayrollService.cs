using System;
using System.Collections.Generic;
using System.Data.Odbc; // ODBC Namespace

public static class EmployeePayrollService
{

    public static EmployeePayroll GetEmployeeByName(string employeeName)
{
    string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD"); // Fetch password from environment variable
    if (string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Password not found in environment variables.");
        return null;
    }

    string connectionString = $"Driver={{MySQL ODBC 9.1 ANSI Driver}};Server=Dhruv;Database=payroll_service;User=root;Password={password};";
    EmployeePayroll employee = null;

    try
    {
        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            connection.Open();

            // SQL query to retrieve employee data by name
            string query = @"SELECT id, name, gender, salary, start_date, phone, address, department, basic_pay, deductions, taxable_pay, income_tax, net_pay
                             FROM employee_payroll WHERE name = ?";

            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", employeeName);

                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new EmployeePayroll(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3),
                            reader.GetDateTime(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7)
                        )
                        {
                            basic_pay = reader.GetDecimal(8),
                            deductions = reader.GetDecimal(9),
                            taxable_pay = reader.GetDecimal(10),
                            income_tax = reader.GetDecimal(11),
                            net_pay = reader.GetDecimal(12)
                        };
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while retrieving employee data: {ex.Message}");
    }

    return employee;
}
public static void InsertEmployee(EmployeePayroll employee)
{
    string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD"); // Fetch password from environment variable
    if (string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Password not found in environment variables.");
        return;
    }

    // Define your connection string (ensure correct password and database details)
    string connectionString = $"Driver={{MySQL ODBC 9.1 ANSI Driver}};Server=Dhruv;Database=payroll_service;User=root;Password={password};";

    try
    {
        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            connection.Open();

            // SQL query to insert employee data, including all required fields (assuming 'id' is auto-increment)
            string query = @"INSERT INTO employee_payroll (name, gender, salary, start_date, phone, address, department, basic_pay, deductions, taxable_pay, income_tax, net_pay)
                             VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Adding parameters to the query for all columns (except 'id' which is auto-increment)
                command.Parameters.AddWithValue("@name", employee.name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@gender", employee.gender.Length == 1 ? employee.gender : "M"); // Default to 'M' if gender is invalid
                command.Parameters.AddWithValue("@salary", employee.salary);
                command.Parameters.AddWithValue("@start_date", employee.start_date);
                command.Parameters.AddWithValue("@phone", employee.phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@address", employee.address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@department", employee.department ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@basic_pay", employee.basic_pay);
                command.Parameters.AddWithValue("@deductions", employee.deductions);
                command.Parameters.AddWithValue("@taxable_pay", employee.taxable_pay);
                command.Parameters.AddWithValue("@income_tax", employee.income_tax);
                command.Parameters.AddWithValue("@net_pay", employee.net_pay);

                // Execute the insert query
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee inserted successfully.");
                }
                else
                {
                    Console.WriteLine("No rows were inserted.");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while inserting employee: {ex.Message}");
    }
}

public static void UpdateEmployeeSalary(string employeeName, decimal newSalary)
{
    string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD"); // Fetch password from environment variable
    if (string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Password not found in environment variables.");
        return;
    }

    string connectionString = $"Driver={{MySQL ODBC 9.1 ANSI Driver}};Server=Dhruv;Database=payroll_service;User=root;Password={password};";

    try
    {
        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            connection.Open();

            // SQL query to update the salary for the employee
            string query = @"UPDATE employee_payroll 
                             SET salary = ?, basic_pay = ?, deductions = ?, taxable_pay = ?, income_tax = ?, net_pay = ?
                             WHERE name = ?";

            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Prepare the parameters
                command.Parameters.AddWithValue("@salary", newSalary);
                command.Parameters.AddWithValue("@basic_pay", newSalary * 0.8m); // Assuming basic pay is 80% of salary
                command.Parameters.AddWithValue("@deductions", newSalary * 0.1m); // Example deduction
                command.Parameters.AddWithValue("@taxable_pay", newSalary - (newSalary * 0.1m)); // Example taxable pay
                command.Parameters.AddWithValue("@income_tax", (newSalary * 0.1m)); // Example income tax (10% of salary)
                command.Parameters.AddWithValue("@net_pay", newSalary - (newSalary * 0.1m) - (newSalary * 0.1m)); // Example net pay
                command.Parameters.AddWithValue("@name", employeeName); // Name of the employee

                // Execute the update query
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee salary updated successfully.");
                }
                else
                {
                    Console.WriteLine("No employee found with the given name.");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while updating salary: {ex.Message}");
    }
}

    // Method to retrieve employee payroll data from the database
    public static List<EmployeePayroll> GetEmployeePayrollData()
    {
        List<EmployeePayroll> payrollList = new List<EmployeePayroll>();
        string password = Environment.GetEnvironmentVariable("MY_APP_PASSWORD"); // Fetch password from environment variable
        if (string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Password not found in environment variables.");
            return payrollList;
        }

        string connectionString = $"Driver={{MySQL ODBC 9.1 ANSI Driver}};Server=Dhruv;Database=payroll_service;User=root;Password={password};";

        try
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                // SQL query to fetch employee payroll data
                string query = "SELECT id, name, gender, salary, start_date, phone, address, department, basic_pay, deductions, taxable_pay, income_tax, net_pay FROM employee_payroll";

                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeePayroll employee = new EmployeePayroll(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3),
                                reader.GetDateTime(4),
                                reader.GetString(5),
                                reader.GetString(6),
                                reader.GetString(7)
                            )
                            {
                                basic_pay = reader.GetDecimal(8),
                                deductions = reader.GetDecimal(9),
                                taxable_pay = reader.GetDecimal(10),
                                income_tax = reader.GetDecimal(11),
                                net_pay = reader.GetDecimal(12)
                            };

                            payrollList.Add(employee);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving employee payroll data: {ex.Message}");
        }

        return payrollList;
    }
}
