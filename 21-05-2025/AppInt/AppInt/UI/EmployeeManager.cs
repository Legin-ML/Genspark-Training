using AppInt.Interfaces;
using AppInt.Models;

namespace AppInt.UI
{
    public class EmployeeManager
    {
        private readonly IEmployeeService _service;

        public EmployeeManager(IEmployeeService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Employee Management ---");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Search Employees");
                Console.WriteLine("3. Exit");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;

                    case "2":
                        SearchEmployees();
                        break;

                    case "3":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void AddEmployee()
        {
            var emp = new Employee();
            emp.TakeEmployeeDetailsFromUser();
            var id = _service.AddEmployee(emp);
            Console.WriteLine(id > 0 ? $"Employee added with ID: {id}" : "Failed to add employee.");
        }

        private void SearchEmployees()
        {
            var model = new SearchModel();

            Console.Write("Enter ID to search (or press Enter to skip): ");
            var idStr = Console.ReadLine();
            if (int.TryParse(idStr, out var searchId))
            {
                model.Id = searchId;
            }

            Console.Write("Enter name to search (or press Enter to skip): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                model.Name = name;
            }

            Console.Write("Enter min age (or press Enter to skip): ");
            var minAgeStr = Console.ReadLine();
            Console.Write("Enter max age (or press Enter to skip): ");
            var maxAgeStr = Console.ReadLine();
            if (int.TryParse(minAgeStr, out int minAge) || int.TryParse(maxAgeStr, out int maxAge))
            {
                model.Age = new Range<int>();
                if (int.TryParse(minAgeStr, out minAge)) model.Age.MinVal = minAge;
                if (int.TryParse(maxAgeStr, out maxAge)) model.Age.MaxVal = maxAge;
            }

            Console.Write("Enter min salary (or press Enter to skip): ");
            var minSalStr = Console.ReadLine();
            Console.Write("Enter max salary (or press Enter to skip): ");
            var maxSalStr = Console.ReadLine();
            if (double.TryParse(minSalStr, out double minSal) || double.TryParse(maxSalStr, out double maxSal))
            {
                model.Salary = new Range<double>();
                if (double.TryParse(minSalStr, out minSal)) model.Salary.MinVal = minSal;
                if (double.TryParse(maxSalStr, out maxSal)) model.Salary.MaxVal = maxSal;
            }

            var results = _service.SearchEmployee(model);
            if (results != null && results.Count > 0)
            {
                Console.WriteLine("\n--- Search Results ---");
                foreach (var e in results)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("------------------");
                }
            }
            else
            {
                Console.WriteLine("No employees found matching the criteria.");
            }
        }
    }
}
