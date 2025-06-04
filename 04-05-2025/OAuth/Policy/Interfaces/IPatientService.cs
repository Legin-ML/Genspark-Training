using FirstAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAPI.Models.DTOs.Patients;

namespace FirstAPI.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> AddPatient(PatientAddRequestDto patient);
        Task<Patient> GetPatientByName(string name);
        Task<ICollection<Patient>> GetAllPatients();
    }
}