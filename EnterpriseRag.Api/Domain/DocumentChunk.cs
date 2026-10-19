namespace EnterpriseRag.Api.Domain
{
    public sealed record DocumentChunk(
        string Id,
        string DocumentId,
        string Content,
        IReadOnlyDictionary<string, string> Metadata);

    public sealed record RetrievedChunk(
        DocumentChunk Chunk,
        double Score);

    public sealed record RagAnswer(
        string Answer,
        IReadOnlyList<string> Sources);
}
