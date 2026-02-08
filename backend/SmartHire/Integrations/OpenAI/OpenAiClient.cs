using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHire.Models.Domain;

namespace SmartHire.Integrations.OpenAI
{
    public class OpenAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAiSettings _settings;
        private readonly ILogger<OpenAiClient> _logger;

        public OpenAiClient(
            HttpClient httpClient,
            IOptions<OpenAiSettings> settings,
            ILogger<OpenAiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new ArgumentException("Gemini ApiKey must be configured.", nameof(settings));

            if (string.IsNullOrWhiteSpace(_settings.Model))
                throw new ArgumentException("Gemini Model must be configured.", nameof(settings));

            // Always use the official host
            _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _settings.ApiKey);
        }

        public async Task<string> GetChatCompletionAsync(IEnumerable<ChatMessage> messages)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            var contents = messages.Select(m => new
            {
                role = NormalizeRole(m.Role),
                parts = new[]
                {
                    new { text = m.Content ?? string.Empty }
                }
            });

            var payload = new { contents };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // v1beta/models/{model}:generateContent?key=API_KEY
            var path = $"v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var response = await _httpClient.PostAsync(path, content).ConfigureAwait(false);
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // Log a short error (no key, no giant blob, unless you really want it)
                _logger.LogError(
                    "Gemini API returned non-success status code {StatusCode}.",
                    (int)response.StatusCode);

                throw new HttpRequestException($"Gemini API error: {response.StatusCode}");
            }

            if (string.IsNullOrWhiteSpace(responseText))
            {
                return string.Empty;
            }

            using var doc = JsonDocument.Parse(responseText);
            var root = doc.RootElement;

            if (!root.TryGetProperty("candidates", out var candidates) ||
                candidates.ValueKind != JsonValueKind.Array ||
                candidates.GetArrayLength() == 0)
            {
                _logger.LogWarning("Gemini response did not contain any candidates.");
                return string.Empty;
            }

            var firstCandidate = candidates[0];

            if (!firstCandidate.TryGetProperty("content", out var contentElement) ||
                !contentElement.TryGetProperty("parts", out var parts) ||
                parts.ValueKind != JsonValueKind.Array ||
                parts.GetArrayLength() == 0)
            {
                _logger.LogWarning("Gemini candidate did not contain any content parts.");
                return string.Empty;
            }

            var text = parts[0].GetProperty("text").GetString();
            return text ?? string.Empty;
        }

        private static string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return "user";

            return role.ToLowerInvariant() switch
            {
                "assistant" => "model",
                "system" => "user",
                _ => "user"
            };
        }
    }
}
