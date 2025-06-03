using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;

namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<string, Appointmnet> _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IRepository<string, Appointmnet> appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<Appointmnet> AddAppointment(AppointmentAddRequestDto appointmentDto)
        {
            try
            {
                var appointment = _mapper.Map<Appointmnet>(appointmentDto);
                appointment.AppointmnetNumber = Guid.NewGuid().ToString(); // Generate a unique ID
                var result = await _appointmentRepository.Add(appointment);

                if (result == null)
                    throw new Exception("Could not create appointment.");

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating appointment: {e.Message}");
            }
        }

        
        public async Task<bool> DeleteAppointment(string appointmentNumber)
        {
            try
            {
                var deletedAppointment = await _appointmentRepository.Delete(appointmentNumber);
                return deletedAppointment != null;
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting appointment: {e.Message}");
            }
        }


    }
}