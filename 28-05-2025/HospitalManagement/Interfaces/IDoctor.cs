using HospitalManagement.Models;
using HospitalManagement.Models.Dtos;

namespace HospitalManagement.Interfaces;

public interface IDoctor
{
    public Task<DoctorDto> GetDoctorByName(string name);
    public Task<ICollection<DoctorDto>> GetDoctorsBySpeciality(string speciality);
    public Task<DoctorDto> AddDoctor(DoctorAddRequestDto doctor);
}