using TrueFeedback.Interfaces;

namespace TrueFeedback.Models;

public class Role : IEntity
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
}