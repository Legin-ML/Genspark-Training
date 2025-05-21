
using System.Xml.Linq;

namespace AppointmentApp.Models
{
    public class Appointment : IComparable<Appointment>, IEquatable<Appointment>
    {

        public int Id { get; set; } = 0;
        public string PatientName { get; set; }

        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        public Appointment()
        { 
        }

        public Appointment(int id, string patientName, int patientAge, DateTime date, string reason)
        {
            Id = id;
            PatientName = patientName;
            PatientAge = patientAge;
            AppointmentDate = date;
            Reason = reason;
        }

        public void TakeAppointmentDetails()
        {
            //Console.WriteLine("Please enter the Appointment ID:");
            //int id;
            //while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            //{
            //    Console.WriteLine("Invalid entry for ID. Please enter a valid Appointment ID.");
            //}
            //Id = id;

            Console.WriteLine("Please enter the Patient Name:");
            PatientName = Console.ReadLine() ?? "";

            Console.WriteLine("Please enter the Patient Age:");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
            {
                Console.WriteLine("Invalid entry for age. Please enter a valid Patient Age.");
            }
            PatientAge = age;

            Console.WriteLine("Please enter the Appointment Date (dd-MM-yyyy):");
            DateTime date;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out date)
                    || date.Date < DateTime.Today)
            {
                Console.WriteLine("Invalid date format. Please enter the date in dd-MM-yyyy format.");
            }
            AppointmentDate = date;

            Console.WriteLine("Please enter the Reason for Appointment:");
            Reason = Console.ReadLine() ?? "";
        }

        public override string ToString()
        {
            return $"Appointment ID: {Id}\n" +
                   $"Patient Name: {PatientName}\n" +
                   $"Patient Age: {PatientAge}\n" +
                   $"Appointment Date: {AppointmentDate:yyyy-MM-dd}\n" +
                   $"Reason: {Reason}";
        }


        public int CompareTo(Appointment? other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public bool Equals(Appointment? other)
        {
            return this.Id == other?.Id;
        }
    }
}
