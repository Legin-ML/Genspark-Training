using TrueFeedback.Contexts;
using TrueFeedback.Models;

namespace TrueFeedback.Repositories;

public class UserRepository(TrueFeedbackContext context, ILogger<User> logger) : Repository<Guid, User>(context, logger);