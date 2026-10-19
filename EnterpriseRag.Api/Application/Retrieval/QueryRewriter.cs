using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace EnterpriseRag.Api.Application.Retrieval
{
    public sealed class QueryRewriter
    {
        private readonly AIAgent _agent;

        public QueryRewriter(IChatClient chatClient)
        {
            _agent = chatClient.AsAIAgent(
                name: "QueryRewriter",
                instructions:
                    """
                Rewrite user questions for enterprise document search.
                Preserve the user's original intent.
                Expand ambiguous technical abbreviations when appropriate.
                Return only the improved search query.
                Do not answer the question.
                """);
        }

        public async Task<string> RewriteAsync(
            string question,
            CancellationToken ct = default)
        {
            var response = await _agent.RunAsync(
                question, cancellationToken: ct);

            return string.IsNullOrWhiteSpace(response.Text)
                ? question
                : response.Text.Trim();
        }
    }
}
