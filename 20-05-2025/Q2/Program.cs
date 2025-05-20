class EmployeePromotion
{
    public List<string> EmployeeNames { get; private set; }

    public EmployeePromotion()
    {
        EmployeeNames = new List<string>();
    }


    public void GetInput()
    {
        Console.WriteLine("Please enter the employee names in the order of their eligibility for promotion:");

        string name;
        while (true)
        {
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                break;
            }

            EmployeeNames.Add(name);
        }
    }

    public void FindEmployeePosition()
    {
        Console.WriteLine("\nPlease enter the name of the employee to check promotion position:");
        string nameToCheck = Console.ReadLine();

        int position = EmployeeNames.IndexOf(nameToCheck);

        if (position != -1)
        {
            Console.WriteLine($"\"{nameToCheck}\" is in position {position + 1} for promotion.");
        }
        else
        {
            Console.WriteLine($"\"{nameToCheck}\" is not in the promotion list.");
        }
    }

    public void RemoveExcessMemory()
    {
        Console.WriteLine($"The current size of the collection is {EmployeeNames.Capacity}");

        EmployeeNames.TrimExcess();
        Console.WriteLine($"The size after removing the extra space is {EmployeeNames.Capacity}");
    }
    public void SortAndDisplayPromotedEmployees()
    {
        Console.WriteLine("\nPromoted employee list:");
        EmployeeNames.Sort();

        foreach (var employee in EmployeeNames)
        {
            Console.WriteLine(employee);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        EmployeePromotion ep = new EmployeePromotion();
        ep.GetInput();
        ep.FindEmployeePosition();
        ep.RemoveExcessMemory();
        ep.SortAndDisplayPromotedEmployees();
    }
}
