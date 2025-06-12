using TrueFeedback.Interfaces;
using TrueFeedback.Models;

namespace TrueFeedback.Services;

public class FeedbackService
{
    private readonly IRepository<Guid, Feedback> _feedbackRepository;

    public FeedbackService(IRepository<Guid, Feedback> feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task<IEnumerable<Feedback>> GetAllAsync(QueryParameters query)
    {
        var feedbacks = (await _feedbackRepository.GetAllAsync())
            .Where(f => !f.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            feedbacks = feedbacks.Where(f =>
                f.Message.Contains(query.Search) ||
                (f.Reply != null && f.Reply.Contains(query.Search)));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            feedbacks = query.SortBy.ToLower() switch
            {
                "created" => query.SortDescending ? feedbacks.OrderByDescending(f => f.Created) : feedbacks.OrderBy(f => f.Created),
                "rating" => query.SortDescending ? feedbacks.OrderByDescending(f => f.Rating) : feedbacks.OrderBy(f => f.Rating),
                _ => feedbacks
            };
        }

        return feedbacks
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }

    public async Task<Feedback> GetByIdAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetAsync(id);
        if (feedback.IsDeleted)
        {
            throw new KeyNotFoundException($"Feedback with id {id} not found");
        }
        return feedback;
    }

    public async Task<Feedback> CreateAsync(Feedback feedback)
    {
        feedback.Created = DateTime.UtcNow;
        feedback.Updated = DateTime.UtcNow;
        feedback.IsDeleted = false;
        return await _feedbackRepository.AddAsync(feedback);
    }

    public async Task<Feedback> UpdateAsync(Guid id, Feedback feedback)
    {
        var existing = await _feedbackRepository.GetAsync(id);
        if (existing.IsDeleted)
        {
            throw new KeyNotFoundException($"Cannot update deleted feedback with id {id}");
        }

        feedback.Id = id;
        feedback.Created = existing.Created;
        feedback.Updated = DateTime.UtcNow;
        return await _feedbackRepository.UpdateAsync(id, feedback);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetAsync(id);
        if (feedback.IsDeleted)
        {
            return true;
        }

        feedback.IsDeleted = true;
        feedback.Updated = DateTime.UtcNow;
        await _feedbackRepository.UpdateAsync(id, feedback);
        return true;
    }
}
