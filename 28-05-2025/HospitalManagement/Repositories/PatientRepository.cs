using HospitalManagement.Contexts;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class PatientRepository : Repository<int, Patient>
    {
        protected PatientRepository(HospitalContext hospitalContext) : base(hospitalContext)
        {
        }

        public override async Task<Patient> Get(int key)
        {
            var patient = await _hospitalContext.Patients.SingleOrDefaultAsync(p => p.Id == key);

            return patient??throw new Exception("No patient with the given ID");
        }

        public override async Task<IEnumerable<Patient>> GetAll()
        {
            var patients = _hospitalContext.Patients;
            if (patients.Any())
                return (await patients.ToListAsync());
            throw new Exception("No Patients in the database");
        }
    }
}