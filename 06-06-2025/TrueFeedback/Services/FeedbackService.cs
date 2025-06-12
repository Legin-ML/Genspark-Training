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

    public async Task<Feedback> CreateAsync(FeedbackCreateReqDto dto, Guid userId)
    {
        var feedback = new Feedback
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = dto.Message,
            Rating = dto.Rating,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            IsDeleted = false
        };
        return await _feedbackRepository.AddAsync(feedback);
    }

    public async Task<Feedback> UpdateAsync(Guid id, FeedbackUpdateReqDto dto, Guid userId, bool isAdmin)
    {
        var feedback = await _feedbackRepository.GetAsync(id);
        if (feedback.IsDeleted)
            throw new KeyNotFoundException($"Feedback {id} not found");

        if (!isAdmin && feedback.UserId != userId)
            throw new UnauthorizedAccessException("You are not allowed to edit this feedback");

        feedback.Message = dto.Message;
        feedback.Rating = dto.Rating;
        feedback.Updated = DateTime.UtcNow;

        return await _feedbackRepository.UpdateAsync(id, feedback);
    }

    public async Task<Feedback> ReplyAsync(Guid id, FeedbackReplyReqDto dto)
    {
        var feedback = await _feedbackRepository.GetAsync(id);
        if (feedback.IsDeleted)
            throw new KeyNotFoundException($"Feedback {id} not found");

        feedback.Reply = dto.Reply;
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
