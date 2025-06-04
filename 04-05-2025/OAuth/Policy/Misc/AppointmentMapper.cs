using AutoMapper;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;

namespace FirstAPI.Misc;

public class AppointmentMapper : Profile
{
    public AppointmentMapper()
    {
        CreateMap<AppointmentAddRequestDto, Appointmnet>();
    }
}
