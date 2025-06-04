using FirstAPI.Models;
using FirstAPI.Models.DTOs;


namespace FirstAPI.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointmnet> AddAppointment(AppointmentAddRequestDto appointmentDto);
        Task<bool> DeleteAppointment(string appointmentNumber);
    }
}