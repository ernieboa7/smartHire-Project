using Microsoft.AspNetCore.Mvc;
using SmartHire.Models.DTOs;
using SmartHire.Services.Interfaces;

namespace SmartHire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiAssistantController : ControllerBase
    {
        private readonly IChatService _chatService;

        public AiAssistantController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponseDto>> Chat(ChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var response = await _chatService.SendMessageAsync(request);
            return Ok(response);
        }
    }
}
