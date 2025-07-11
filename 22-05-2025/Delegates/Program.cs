﻿class Program
{
    public delegate void MyDelegate(int num1, int num2);

    List<Employee> employees = new List<Employee>()
        {
            new Employee(101,30, "John Doe",  50000),
            new Employee(102, 25,"Jane Smith",  60000),
            new Employee(103,35, "Sam Brown",  70000)
        };

    public void Add(int n1, int n2)
    {
        int sum = n1 + n2;
        Console.WriteLine($"The sum of {n1} and {n2} is {sum}");
    }
    public void Product(int n1, int n2)
    {
        int prod = n1 * n2;
        Console.WriteLine($"The product of {n1} and {n2} is {prod}");
    }
    public void Subtract(int n1, int n2)
    {
        int sub = n1 - n2;
        System.Console.WriteLine($"The difference of {n1} and {n2} is {sub}");
    }

    void FindEmployee()
    {
        int empId = 102;
        Predicate<Employee> predicate = e => e.Id == empId;
        Employee? emp = employees.Find(predicate);
        Console.WriteLine(emp.ToString()??"No such employee");
    }
    void SortEmployee()
    {
        var sortedEmployees = employees.OrderBy(e => e.Name);
        foreach (var emp in sortedEmployees)
        {
            Console.WriteLine(emp.ToString());
        }
    }
    Program()
    {
        MyDelegate del = new MyDelegate(Add);
        del += Product;
        del += Subtract;
        Action<int, int> delAction = Add;
        delAction += Product;
        delAction += Subtract;
        del(30, 10);
        delAction(30, 20);
    }
    static void Main(string[] args)
    {
        Program program = new();
        program.FindEmployee();
        program.SortEmployee();
    }
}
