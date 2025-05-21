using AppointmentApp.Interfaces;
using AppointmentApp.Models;
using AppointmentApp.Repositories;
using AppointmentApp.Services;
using AppointmentApp.UI;

namespace AppointmentApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepository<int, Appointment> repo = new AppointmentRepository();
            IAppointmentService service = new AppointmentService(repo);
            AppointmentManager manager = new AppointmentManager(service);

            manager.Run();
        }
    }
}
