using AppointmentApp.Interfaces;
using AppointmentApp.Models;

namespace AppointmentApp.UI
{
    public class AppointmentManager
    {
        private readonly IAppointmentService _service;

        public AppointmentManager(IAppointmentService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Appointment Management ---");
                Console.WriteLine("1. Add Appointment");
                Console.WriteLine("2. Search Appointments");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddAppointment();
                        break;
                    case "2":
                        SearchAppointments();
                        break;
                    case "3":
                        Console.WriteLine("Exiting Appointment Manager.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void AddAppointment()
        {
            try
            {
                var appointment = new Appointment(0, "", 0, DateTime.Now, "");
                appointment.TakeAppointmentDetails();
                int id = _service.AddAppointment(appointment);

                if (id > 0)
                {
                    Console.WriteLine($"Appointment added successfully with ID: {id}");
                }
                else
                {
                    Console.WriteLine("Failed to add appointment.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SearchAppointments()
        {
            try
            {
                var searchModel = new AppointmentSearchModel();

                Console.Write("Enter patient name (or press Enter to skip): ");
                string? name = Console.ReadLine();
                searchModel.PatientName = null;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    searchModel.PatientName = name;
                }

                Console.Write("Enter appointment date (dd-MM-yyyy) (or press Enter to skip): ");
                string? dateInput = Console.ReadLine();
                searchModel.AppointmentDate = null;
                if (DateTime.TryParseExact(dateInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    searchModel.AppointmentDate = date;
                }

                Console.Write("Enter minimum patient age (or press Enter to skip): ");
                string? minAgeStr = Console.ReadLine();
                Console.Write("Enter maximum patient age (or press Enter to skip): ");
                string? maxAgeStr = Console.ReadLine();
                searchModel.AgeRange = null;
                if (int.TryParse(minAgeStr, out int minAge) || int.TryParse(maxAgeStr, out int maxAge))
                {
                    searchModel.AgeRange = new Range<int>();
                    if (int.TryParse(minAgeStr, out minAge)) searchModel.AgeRange.MinVal = minAge;
                    if (int.TryParse(maxAgeStr, out maxAge)) searchModel.AgeRange.MaxVal = maxAge;
                }

                var results = _service.SearchAppointment(searchModel);
                if (results != null && results.Any())
                {
                    Console.WriteLine("\n--- Search Results ---");
                    foreach (var a in results)
                    {
                        Console.WriteLine(a);
                        Console.WriteLine("-------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No appointments found matching the criteria.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
