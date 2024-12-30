using System;

public class EmployeePayroll
{
    public int id { get; set; }
    public string name { get; set; }
    public string gender { get; set; }
    public decimal salary { get; set; }
    public DateTime start_date { get; set; }
    public string phone { get; set; }
    public string address { get; set; }
    public string department { get; set; }
    public decimal basic_pay { get; set; }
    public decimal deductions { get; set; }
    public decimal taxable_pay { get; set; }
    public decimal income_tax { get; set; }
    public decimal net_pay { get; set; }

    // Constructor for backward compatibility
    public EmployeePayroll(int id, string name, string gender, decimal salary, DateTime start_date, string phone, string address, string department)
    {
        this.id = id;
        this.name = name;
        this.gender = gender;
        this.salary = salary;
        this.start_date = start_date;
        this.phone = phone;
        this.address = address;
        this.department = department;

        // Assuming BasicPay is equal to Salary for simplicity
        this.basic_pay = salary;

        // Assuming Deductions is 10% for simplicity
        this.deductions = basic_pay * 0.1m;

        // Assuming TaxablePay is Salary - Deductions
        this.taxable_pay = salary - this.deductions;

        // Assuming IncomeTax is 20% of TaxablePay
        this.income_tax = this.taxable_pay * 0.2m;

        // Net Pay calculation
        this.net_pay = this.basic_pay - this.deductions - this.income_tax;
    }

    // Constructor without certain fields for backward compatibility
    public EmployeePayroll(int id, string name, string gender, decimal salary)
        : this(id, name, gender, salary, DateTime.MinValue, "Unknown", "Unknown", "Unknown")
    { }
}
