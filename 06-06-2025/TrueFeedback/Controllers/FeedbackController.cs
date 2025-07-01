using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using TrueFeedback.Hubs;
using TrueFeedback.Models;
using TrueFeedback.Models.DTOs;
using TrueFeedback.Services;

namespace TrueFeedback.Controllers;

[ApiController]
[Route("api/v1/feedbacks")]
public class FeedbackController : ControllerBase
{
    private readonly FeedbackService _feedbackService;
    private readonly IHubContext<TrueFeedbackHub> _hubContext;

    public FeedbackController(FeedbackService feedbackService,  IHubContext<TrueFeedbackHub> hubContext)
    {
        _feedbackService = feedbackService;
        _hubContext = hubContext;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<Feedback>>> GetAll([FromQuery] QueryParameters query)
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
        await _hubContext.Clients.All.SendAsync("FeedbackPosted", feedback);
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
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
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
            await _hubContext.Clients.All.SendAsync("FeedbackReplied", updated);
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
