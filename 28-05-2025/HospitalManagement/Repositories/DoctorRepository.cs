using HospitalManagement.Contexts;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories;

public class DoctorRepository : Repository<int, Doctor>
{
    public DoctorRepository(HospitalContext hospitalContext) : base(hospitalContext)
    {
    }

    public override async Task<Doctor> Get(int key)
    {
        var doctor = await _hospitalContext.Doctors.SingleOrDefaultAsync(d => d.Id == key);

        return doctor??throw new Exception("No Doctor with the given ID");
    }

    public override async Task<IEnumerable<Doctor>> GetAll()
    {
        var doctors = _hospitalContext.Doctors;
        if (!doctors.Any())
            throw new Exception("No Doctors in the database");
        return  await _hospitalContext.Doctors.ToListAsync();
    }
}