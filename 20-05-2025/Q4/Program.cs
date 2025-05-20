
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

    public void ModifyEmployee(int id)
{
    var employee = GetEmployeeById(id);
    if (employee != null)
    {
        Console.WriteLine("Modify details for Employee ID: " + id);
        Console.WriteLine($"Enter new Name (current: {employee.Name}):");
        employee.Name = Console.ReadLine();
        
        Console.WriteLine($"Enter new Age (current: {employee.Age}):");
        int newAge;
        while (!int.TryParse(Console.ReadLine(), out newAge) || newAge <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid age (positive integer):");
        }
        employee.Age = newAge; 
        
        Console.WriteLine($"Enter new Salary (current: {employee.Salary}):");
        double newSalary;
        while (!double.TryParse(Console.ReadLine(), out newSalary) || newSalary <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid salary (positive number):");
        }
        employee.Salary = newSalary;  
    }
}


    public void DeleteEmployee(int id)
    {
        if (employees.ContainsKey(id))
        {
            employees.Remove(id);
            Console.WriteLine($"Employee with ID {id} has been deleted.");
        }
        else
        {
            Console.WriteLine("\nEmployee not found.");
        }
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
            Console.WriteLine("3. Modify Employee Details");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");

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
                    Console.Write("Enter Employee ID to search: ");
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
                    Console.WriteLine("Enter Employee ID to modify:");
                    int modifyId;
                    while (!int.TryParse(Console.ReadLine(), out modifyId) || modifyId <= 0)
                    {
                        Console.Write("Invalid input. Please enter a valid employee ID:");
                    }
                    em.ModifyEmployee(modifyId);
                    break;

                case 4:
                    Console.WriteLine("Enter Employee ID to delete:");
                    int deleteId;
                    while (!int.TryParse(Console.ReadLine(), out deleteId) || deleteId <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid employee ID:");
                    }
                    em.DeleteEmployee(deleteId);
                    break;

                case 5:
                    loopValid = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
}
