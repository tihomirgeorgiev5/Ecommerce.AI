using AiService.Models;

namespace AiService.Services
{
    /// <summary>
    /// Ask Chatbot a question
    /// It will first in product db and if not found, fallback to web search + LLM to generate answer
    /// </summary>
    public interface IChatService
    {
        Task<ChatResponse> AskAsync(
            string userQuery,
            CancellationToken cancellationToken = default,
            IEnumerable<(string Role, string Content)>? chatHistory = null);
    }
}
