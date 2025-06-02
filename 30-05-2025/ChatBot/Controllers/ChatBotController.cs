using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly ChatBotService _chatBotService;

        public ChatBotController(ChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost]
        public IActionResult Ask([FromBody] string userInput)
        {
            var response = _chatBotService.GetBestAnswer(userInput);
            return Ok(new { answer = response });
        }
    }

}
