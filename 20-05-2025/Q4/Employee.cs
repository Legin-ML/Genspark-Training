using System;
using System.Collections.Generic;
using System.Linq;

class Employee : IComparable<Employee>
{
    int id, age;
    string name;
    double salary;

    public Employee() { }

    public Employee(int id, int age, string name, double salary)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.salary = salary;
    }

    public void TakeEmployeeDetailsFromUser()
    {
        Console.WriteLine("Please enter the employee ID (integer):");
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid employee ID (positive integer):");
        }

        Console.WriteLine("Please enter the employee name:");
        name = Console.ReadLine();

        Console.WriteLine("Please enter the employee age (integer):");
        while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid age (positive integer):");
        }

        Console.WriteLine("Please enter the employee salary (double):");
        while (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid salary (positive number):");
        }
    }

    public override string ToString()
    {
        return $"Employee ID : {id}\nName : {name}\nAge : {age}\nSalary : {salary}";
    }

    public int CompareTo(Employee other)
    {
        return salary.CompareTo(other.salary);
    }

    public int Id { get => id; set => id = value; }
    public int Age { get => age; set => age = value; }
    public string Name { get => name; set => name = value; }
    public double Salary { get => salary; set => salary = value; }
}