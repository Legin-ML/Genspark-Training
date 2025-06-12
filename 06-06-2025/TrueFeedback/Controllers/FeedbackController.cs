using Microsoft.AspNetCore.Mvc;
using TrueFeedback.Models;
using TrueFeedback.Services;

namespace TrueFeedback.Controllers;

[ApiController]
[Route("api/v1/feedbacks")]
public class FeedbackController : ControllerBase
{
    private readonly FeedbackService _feedbackService;

    public FeedbackController(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAll([FromQuery] QueryParameters query)
    {
        var feedbacks = await _feedbackService.GetAllAsync(query);
        return Ok(feedbacks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> GetById(Guid id)
    {
        try
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            return Ok(feedback);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
    {
        var created = await _feedbackService.CreateAsync(feedback);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Feedback>> Update(Guid id, [FromBody] Feedback feedback)
    {
        try
        {
            var updated = await _feedbackService.UpdateAsync(id, feedback);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _feedbackService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}