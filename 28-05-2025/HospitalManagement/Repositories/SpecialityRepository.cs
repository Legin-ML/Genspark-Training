using HospitalManagement.Contexts;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories;

public class SpecialityRepository : Repository<int, Speciality>
{
    public SpecialityRepository(HospitalContext hospitalContext) : base(hospitalContext)
    {
    }

    public override async Task<Speciality> Get(int key)
    {
        var speciality = await _hospitalContext.Specialities.FindAsync(key);
        return speciality??throw new Exception("Speciality not found");
    }

    public override async Task<IEnumerable<Speciality>> GetAll()
    {
        var specialities =  _hospitalContext.Specialities;
        if (specialities.Any())
            return await specialities.ToListAsync();
        throw new Exception("No specialities are found");
    }
}