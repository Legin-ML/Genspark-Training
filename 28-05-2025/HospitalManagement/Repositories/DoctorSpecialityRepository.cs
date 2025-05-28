using HospitalManagement.Contexts;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories;

public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
{
    public DoctorSpecialityRepository(HospitalContext hospitalContext) : base(hospitalContext)
    {
    }

    public override async Task<DoctorSpeciality> Get(int key)
    {
        var doctorSpeciality =  await _hospitalContext.DoctorSpecialities.SingleOrDefaultAsync(ds => ds.SpecialityId == key);
        return doctorSpeciality??throw new Exception("No speciality with the given ID");
    }

    public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
    {
        var doctorSpecialities = await _hospitalContext.DoctorSpecialities.ToListAsync();
        if (doctorSpecialities.Any())
            return doctorSpecialities;
        throw new Exception("No Doctor Specialities in the database");
    }
}