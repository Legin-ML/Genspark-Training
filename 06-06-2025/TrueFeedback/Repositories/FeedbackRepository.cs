using TrueFeedback.Contexts;
using TrueFeedback.Models;

namespace TrueFeedback.Repositories;

public class FeedbackRepository(TrueFeedbackContext context, ILogger<Feedback> logger)
    : Repository<Guid, Feedback>(context, logger);