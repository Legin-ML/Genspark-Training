
class EmployeeManager
{
    private Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

    public void AddEmployee(Employee employee)
    {
        if (!employees.ContainsKey(employee.Id))
        {
            employees.Add(employee.Id, employee);
        }
        else
        {
            Console.WriteLine("Employee ID already exists.");
        }
    }

    public Employee GetEmployeeById(int id)
    {
        if (employees.ContainsKey(id))
        {
            return employees[id];
        }
        else
        {
            Console.WriteLine("Employee not found.");
            return null;
        }
    }

    public List<Employee> GetAllEmployeesSortedBySalary()
    {
        List<Employee> res = employees.Values.ToList();
        res.Sort();
        return res;
    }

    public List<Employee> GetEmployeesByName(string name)
    {
        return employees.Values.Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Employee> GetEmployeesOlderThan(Employee givenEmployee)
    {
        return employees.Values.Where(e => e.Age > givenEmployee.Age).ToList();
    }
}

class Program
{
    static void Main(string[] args)
    {
        EmployeeManager em = new EmployeeManager();
        bool loopValid = true;

        while (loopValid)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Add new Employee");
            Console.WriteLine("2. Get Employee by ID");
            Console.WriteLine("3. Sort Employees by Salary");
            Console.WriteLine("4. Find Employees by Name");
            Console.WriteLine("5. Find Employees older than a specific Employee");
            Console.WriteLine("6. Exit");
            
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 6)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 6:");
            }

            switch (choice)
            {
                case 1:
                    Employee newEmployee = new Employee();
                    newEmployee.TakeEmployeeDetailsFromUser();
                    em.AddEmployee(newEmployee);
                    break;

                case 2:
                    Console.WriteLine("Enter Employee ID to search:");
                    int inp;
                    while (!int.TryParse(Console.ReadLine(), out inp) || inp <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid employee ID:");
                    }
                    Employee found = em.GetEmployeeById(inp);
                    if (found != null)
                    {
                        Console.WriteLine(found);
                    }
                    break;

                case 3:
                    List<Employee> sorted = em.GetAllEmployeesSortedBySalary();
                    Console.WriteLine("Employees sorted by salary:");
                    foreach (var e in sorted)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 4:
                    Console.WriteLine("Enter the name to search for:");
                    string name = Console.ReadLine();
                    List<Employee> employees = em.GetEmployeesByName(name);
                    Console.WriteLine($"Employees with the name '{name}':");
                    foreach (var e in employees)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 5:
                    Console.WriteLine("Enter Employee ID to compare age:");
                    int compare;
                    while (!int.TryParse(Console.ReadLine(), out compare) || compare <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid employee ID:");
                    }
                    Employee given = em.GetEmployeeById(compare);
                    if (given != null)
                    {
                        List<Employee> olderEmployees = em.GetEmployeesOlderThan(given);
                        if (olderEmployees.Count() < 1)
                        {
                            Console.WriteLine($"No employees found older than {given.Name}");
                            break;
                        }
                        Console.WriteLine($"Employees older than {given.Name}:");
                        foreach (var e in olderEmployees)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    break;

                case 6:
                    loopValid = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
}