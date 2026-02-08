using SmartHire.Integrations.OpenAI;
using SmartHire.Models.Domain;
using SmartHire.Models.DTOs;
using SmartHire.Repositories.Interfaces;
using SmartHire.Services.Interfaces;

namespace SmartHire.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly OpenAiClient _openAiClient;

        public ChatService(
            IChatSessionRepository chatSessionRepository,
            OpenAiClient openAiClient)
        {
            _chatSessionRepository = chatSessionRepository;
            _openAiClient = openAiClient;
        }

        public async Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request)
        {
            ChatSession session;

            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                session = new ChatSession
                {
                    Id = Guid.NewGuid().ToString(),
                    CandidateId = request.CandidateId,
                    JobId = request.JobId
                };
            }
            else
            {
                session = await _chatSessionRepository.GetByIdAsync(request.SessionId)
                          ?? new ChatSession
                          {
                              Id = request.SessionId,
                              CandidateId = request.CandidateId,
                              JobId = request.JobId
                          };
            }

            var userMessage = new ChatMessage
            {
                Role = "user",
                Content = request.Message,
                TimestampUtc = DateTime.UtcNow
            };
            session.Messages.Add(userMessage);
            session.UpdatedAtUtc = DateTime.UtcNow;

            // Call OpenAI
            var assistantReply = await _openAiClient.GetChatCompletionAsync(session.Messages);

            var assistantMessage = new ChatMessage
            {
                Role = "assistant",
                Content = assistantReply,
                TimestampUtc = DateTime.UtcNow
            };
            session.Messages.Add(assistantMessage);
            session.UpdatedAtUtc = DateTime.UtcNow;

            await _chatSessionRepository.UpsertAsync(session);

            return new ChatResponseDto
            {
                SessionId = session.Id,
                AssistantReply = assistantReply,
                Messages = session.Messages.Select(m => new ChatMessageDto
                {
                    Role = m.Role,
                    Content = m.Content,
                    TimestampUtc = m.TimestampUtc
                }).ToList()
            };
        }
    }
}
