namespace SmartHire.Integrations.OpenAI
{
    public class OpenAiSettings
    {
        // Your Gemini API key
        public string ApiKey { get; set; } = null!;

        // Gemini model name — choose the one you want
        // "gemini-1.5-flash"  (fast, cheap)
        // "gemini-1.5-pro"    (more capable)
        public string Model { get; set; } = "gemini-2.5-flash";

        // Correct Gemini base URL (v1beta is required!)
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
    }
}
