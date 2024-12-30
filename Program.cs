using System;
using System.Collections.Generic;

namespace EmployeePayrollServices
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example: Inserting a new employee
           
            List<EmployeePayroll> payrollList = EmployeePayrollService.GetEmployeePayrollData();
            foreach (var payroll in payrollList)
            {
                Console.WriteLine($"ID: {payroll.id}, Name: {payroll.name}, Gender: {payroll.gender}, Salary: {payroll.salary}, Start Date: {payroll.start_date.ToShortDateString()}");
            }

            // Update salary for "Terisa" and related fields (basic pay, deductions, etc.)
            decimal newSalary = 3000000.00m;
            Console.WriteLine($"\nUpdating salary for Terisa to {newSalary}...");
            EmployeePayrollService.UpdateEmployeeSalary("Terisa", newSalary);

            // Fetch and display the updated employee data from DB
            Console.WriteLine("\nEmployee data after salary update:");
            payrollList = EmployeePayrollService.GetEmployeePayrollData();
            foreach (var payroll in payrollList)
            {
                Console.WriteLine($"ID: {payroll.id}, Name: {payroll.name}, Gender: {payroll.gender}, Salary: {payroll.salary}, Start Date: {payroll.start_date.ToShortDateString()}");
            }

            Console.ReadKey();
        }
    }
}
