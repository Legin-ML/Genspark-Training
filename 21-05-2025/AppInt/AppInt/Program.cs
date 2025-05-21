using AppInt.Models;
using AppInt.Services;
using AppInt.Interfaces;
using AppInt.Repositories;
using AppInt.UI;

class Program
{
    static void Main()
    {
        IRepository<int, Employee> repo = new EmployeeRepository();
        IEmployeeService service = new EmployeeService(repo);
        EmployeeManager manager = new EmployeeManager(service);

        manager.Run();
    }
}
