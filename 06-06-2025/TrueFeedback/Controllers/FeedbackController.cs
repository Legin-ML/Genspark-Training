using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrueFeedback.Models;
using TrueFeedback.Models.DTOs;
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
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetAll([FromQuery] QueryParameters query)
    {
        var feedbacks = await _feedbackService.GetAllAsync(query);
        return Ok(feedbacks);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
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
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<Feedback>> Create([FromBody] FeedbackCreateReqDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var feedback = await _feedbackService.CreateAsync(dto, Guid.Parse(userId));
        return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<Feedback>> Update(Guid id, [FromBody] FeedbackUpdateReqDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        try
        {
            var updated = await _feedbackService.UpdateAsync(id, dto, userId, User.IsInRole("Admin"));
            return Ok(updated);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}/reply")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Feedback>> Reply(Guid id, [FromBody] FeedbackReplyReqDto dto)
    {
        try
        {
            var updated = await _feedbackService.ReplyAsync(id, dto);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _feedbackService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
