using EnterpriseRag.Api.Domain;
using Microsoft.Extensions.AI;

namespace EnterpriseRag.Api.Application.Retrieval
{
    //public sealed class RetrievalService(
    //    IEmbeddingGenerator<string, Embedding<float>> embeddings,
    //    IVectorStore vectorStore)
    //{
    //    public async Task<IReadOnlyList<RetrievedChunk>> RetrieveAsync(
    //        string query,
    //        int topK = 5,
    //        CancellationToken ct = default)
    //    {
    //        var generated = await embeddings.GenerateAsync(
    //            [query], cancellationToken: ct);

    //        var queryVector = generated.Single().Vector;

    //        return await vectorStore.SearchAsync(
    //            queryVector, topK, ct);
    //    }
    //}

    public sealed class RetrievalService(
    QueryRewriter queryRewriter,
    IEmbeddingGenerator<string, Embedding<float>> embeddings,
    IVectorStore vectorStore)
    {
        public async Task<IReadOnlyList<RetrievedChunk>> RetrieveAsync(
            string query,
            int topK = 5,
            CancellationToken ct = default)
        {
            var rewritten = await queryRewriter.RewriteAsync(query, ct);

            var generated = await embeddings.GenerateAsync(
                [rewritten], cancellationToken: ct);

            return await vectorStore.SearchAsync(
                generated.Single().Vector, topK, ct);
        }
    }
}
