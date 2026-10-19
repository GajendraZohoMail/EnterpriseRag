namespace EnterpriseRag.Api.Application.Ingestion
{
    public sealed class TextChunker
    {
        public IReadOnlyList<string> Split(
            string text,
            int chunkSize = 1200,
            int overlap = 150)
        {
            if (chunkSize <= 0 || overlap < 0 || overlap >= chunkSize)
                throw new ArgumentOutOfRangeException(
                    nameof(chunkSize), "Require 0 <= overlap < chunkSize.");

            var result = new List<string>();
            var start = 0;

            while (start < text.Length)
            {
                var length = Math.Min(chunkSize, text.Length - start);

                // Prefer splitting at a nearby word boundary.
                var end = start + length;
                if (end < text.Length)
                {
                    var boundary = text.LastIndexOf(
                        ' ', end - 1, length);

                    if (boundary > start + chunkSize / 2)
                        length = boundary - start;
                }

                var chunk = text.Substring(start, length).Trim();

                if (!string.IsNullOrWhiteSpace(chunk))
                    result.Add(chunk);

                start += Math.Max(1, length - overlap);
            }

            return result;
        }
    }
}
