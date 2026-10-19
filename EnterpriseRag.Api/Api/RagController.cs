using EnterpriseRag.Api.Application.Generation;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseRag.Api.Api
{
    [ApiController]
    [Route("api/rag")]
    public sealed class RagController(RagService ragService) : ControllerBase
    {
        [HttpPost("ask")]
        public async Task<IActionResult> Ask(
            [FromBody] AskRequest request,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required.");

            var result = await ragService.AskAsync(
                request.Question, ct);

            return Ok(result);
        }
    }

    public sealed record AskRequest(string Question);
}
