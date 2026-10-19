using EnterpriseRag.Api.Domain;
using Microsoft.Extensions.AI;

namespace EnterpriseRag.Api.Application.Ingestion
{
    public sealed class IndexingService(
        IDocumentLoader loader,
        TextChunker chunker,
        IEmbeddingGenerator<string, Embedding<float>> embeddings,
        IVectorStore vectorStore)
    {
        public async Task IndexAllAsync(CancellationToken ct)
        {
            await foreach (var document in loader.LoadAsync(ct))
            {
                var parts = chunker.Split(document.Text);

                for (var i = 0; i < parts.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var content = parts[i];

                    var metadata =
                        new Dictionary<string, string>(document.Metadata)
                        {
                            ["chunkIndex"] = i.ToString(),
                            ["sourceId"] = document.DocumentId
                        };

                    var chunk = new DocumentChunk(
                        $"{document.DocumentId}-{i}",
                        document.DocumentId,
                        content,
                        metadata);

                    var generated = await embeddings.GenerateAsync(
                        [content], cancellationToken: ct);

                    var embedding = generated.Single().Vector;

                    await vectorStore.UpsertAsync(
                        chunk,
                        embedding,
                        ct);
                }
            }
        }
    }
}
