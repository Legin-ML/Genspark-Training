using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Authorization;

public class HasEnoughExperienceRequirement : IAuthorizationRequirement
{
    public int Years { get; }
    public HasEnoughExperienceRequirement(int years)
    {
        Years = years;
    }   
}