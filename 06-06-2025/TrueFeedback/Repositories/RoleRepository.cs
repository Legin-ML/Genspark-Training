using TrueFeedback.Contexts;
using TrueFeedback.Models;

namespace TrueFeedback.Repositories;

public class RoleRepository(TrueFeedbackContext context, ILogger<Role> logger) : Repository<Guid, Role>(context, logger);