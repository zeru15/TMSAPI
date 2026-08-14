using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace TmsApi.Api.Controllers.V2;

[ApiController]
[Route("api/v{version:apiVersion}/transcripts")]
[ApiVersion("2.0")]
public class TranscriptsController : ControllerBase
{
    [HttpPost]
    [EnableRateLimiting("transcripts")]
    public async Task<IActionResult> RequestTranscript(
        CancellationToken ct)
    {
        // Temporary stub for Exercise 4.
        // Exercise 5 will replace this with the real
        // background transcript workflow.

        // await Task.Delay(5000, ct);

        // temporary delay so we can observe concurrency limiting
        await Task.Delay(TimeSpan.FromSeconds(5), ct);

        return Ok();
    }
}