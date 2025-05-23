using Microsoft.AspNetCore.Mvc;
namespace legal_document_analyzer.Presentation.Controllers
{
    using legal_document_analyzer.Application.DTOs;
    using legal_document_analyzer.Domain.Entities;
    using legal_document_analyzer.Domain.Repositories;
    using legal_document_analyzer.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using legal_document_analyzer.Application.Services;
    using legal_document_analyzer.Domain.ValueObjects;
    using legal_document_analyzer.Infrastructure.Extensions;
    using Microsoft.Extensions.Configuration.UserSecrets;
    using System.Security.Claims;
    [ApiController]
    [Route("api/chat")]
    [Authorize] 
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IAzureOpenAiService _azureOpenAiService;

        public ChatController(IChatRepository chatRepository, IAzureOpenAiService azureOpenAiService)
        {
            _chatRepository = chatRepository;
            _azureOpenAiService = azureOpenAiService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto dto)
        {

            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"The user id could be {userIdStringOrNot}");
            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");

            var session = await _chatRepository.GetActiveSessionAsync(userIdStringOrNot) ?? await _chatRepository.CreateSessionAsync(userIdStringOrNot);

            // Store user message
            await _chatRepository.AddMessageAsync(session.Id, "user", dto.Content, dto.InputMode);

            // Retrieve full conversation to pass to Azure OpenAI
            var history = await _chatRepository.GetMessagesAsync(session.Id);

            // Get assistant response
            var assistantReply = await _azureOpenAiService.GetCompletionAsync(history);

            // Store assistant message
            await _chatRepository.AddMessageAsync(session.Id, "assistant", assistantReply, "text");

            return Ok(new { reply = assistantReply });
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");


            var session = await _chatRepository.GetActiveSessionAsync(userIdStringOrNot);
            if (session == null) return Ok(new List<MyChatMessage>());

            var messages = await _chatRepository.GetMessagesAsync(session.Id);
            return Ok(messages);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetChat()
        {
            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");

            var session = await _chatRepository.GetActiveSessionAsync(userIdStringOrNot);
            if (session != null)
            {
                await _chatRepository.EndSessionAsync(session.Id);
            }

            return Ok();
        }
    }

}
