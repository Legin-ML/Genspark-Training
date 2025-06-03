using System.Security.Claims;
using FirstAPI.Authorization;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Policies.Handlers;

public class HasEnoughExperienceHandler : AuthorizationHandler<HasEnoughExperienceRequirement>
{
    protected readonly IRepository<int, Doctor> _doctorRepostitory;

    public HasEnoughExperienceHandler(IRepository<int, Doctor> doctorRepository)
    {
        _doctorRepostitory = doctorRepository;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasEnoughExperienceRequirement requirement)
    {
        var doctorIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "DoctorId");
        if (doctorIdClaim == null)
        {
            throw new System.Exception("Claim not found");
            return;
        }
        
        int doctorId = int.Parse(doctorIdClaim.Value);
        var doctor = await _doctorRepostitory.Get(doctorId);
        
        if(doctor != null && doctor.YearsOfExperience > requirement.Years)
        {
            context.Succeed(requirement);
        }
    }
}