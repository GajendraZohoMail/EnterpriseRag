using EnterpriseRag.Api.Domain;

namespace EnterpriseRag.Api.Application
{
    public interface IVectorStore
    {
        Task UpsertAsync(
            DocumentChunk chunk,
            ReadOnlyMemory<float> embedding,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<RetrievedChunk>> SearchAsync(
            ReadOnlyMemory<float> queryEmbedding,
            int topK,
            CancellationToken cancellationToken);
    }

    public interface IDocumentLoader
    {
        IAsyncEnumerable<(string DocumentId, string Text,
            IReadOnlyDictionary<string, string> Metadata)> LoadAsync(
            CancellationToken cancellationToken);
    }

    public sealed class DummyDocumentLoader : IDocumentLoader
    {
        public IAsyncEnumerable<(string DocumentId, string Text, IReadOnlyDictionary<string, string> Metadata)> LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class DummyVectorStore : IVectorStore
    {
        public Task<IReadOnlyList<RetrievedChunk>> SearchAsync(ReadOnlyMemory<float> queryEmbedding, int topK, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpsertAsync(DocumentChunk chunk, ReadOnlyMemory<float> embedding, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}