using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentApp.Interfaces;
using AppointmentApp.Models;

namespace AppointmentApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        IRepository<int, Appointment> _appointmentRepository;

        public AppointmentService(IRepository<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var result = _appointmentRepository.Add(appointment);
                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        public List<Appointment>? SearchAppointment(AppointmentSearchModel asm)
        {
            try
            {
                var resAppointments = _appointmentRepository.GetAll();
                resAppointments = SearchByName(resAppointments, asm.PatientName);
                resAppointments = SearchByDate(resAppointments, asm.AppointmentDate);
                resAppointments = SearchByAge(resAppointments, asm.AgeRange);

                if (resAppointments != null && resAppointments.Any())
                {
                    return resAppointments.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private ICollection<Appointment> SearchByAge(ICollection<Appointment> resAppointments, Range<int> ageRange)
        {
            if(ageRange == null || resAppointments == null || resAppointments.Count == 0)
            {
                return resAppointments;
            }
            
            return resAppointments.Where(e => e.PatientAge >= ageRange.MinVal && e.PatientAge <= ageRange.MaxVal).ToList();
        }

        private ICollection<Appointment> SearchByDate(ICollection<Appointment> resAppointments, DateTime? appointmentDate)
        {
            if (appointmentDate == null || resAppointments == null || resAppointments.Count == 0)
            {
                return resAppointments;
            }
            return resAppointments.Where(e => e.AppointmentDate.Date == appointmentDate.Value.Date).ToList();
        }

        private ICollection<Appointment> SearchByName(ICollection<Appointment> resAppointments, string? patientName)
        {
            if (patientName == null || resAppointments == null || resAppointments.Count == 0)
            {
                return resAppointments;
            }
            return resAppointments.Where(e => e.PatientName.Equals(patientName, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
