using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Transcripts;
using TmsApi.Infrastructure.Transcripts;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/transcripts")]
public sealed class TranscriptsController : ControllerBase
{
    private readonly Channel<TranscriptJobRequest> _channel;
    private readonly ITranscriptStatusStore _store;

    public TranscriptsController(
        Channel<TranscriptJobRequest> channel,
        ITranscriptStatusStore store)
    {
        _channel = channel;
        _store = store;
    }

    [HttpPost]
[HttpPost]
public async Task<IActionResult> GenerateTranscript(
    GenerateTranscriptRequest request,
    CancellationToken cancellationToken)
{
    if (Request.Headers.TryGetValue(
            "Idempotency-Key", out var keyValues))
    {
        var key = keyValues.ToString();

        if (_store.TryGetJobByKey(key, out var existingJobId))
        {
            return Accepted(new
            {
                jobId = existingJobId,
                statusUrl = $"/api/transcripts/{existingJobId}",
                reused = true
            });
        }

        var jobId = Guid.NewGuid();

        _store.Create(jobId, request.StudentId, request.Format);
        _store.SaveKey(key, jobId);

        await _channel.Writer.WriteAsync(
            new TranscriptJobRequest(
                jobId,
                request.StudentId,
                request.Format),
            cancellationToken);

        return Accepted(new
        {
            jobId,
            statusUrl = $"/api/transcripts/{jobId}",
            reused = false
        });
    }

    var newJobId = Guid.NewGuid();

    _store.Create(newJobId, request.StudentId, request.Format);

    await _channel.Writer.WriteAsync(
        new TranscriptJobRequest(
            newJobId,
            request.StudentId,
            request.Format),
        cancellationToken);

    return Accepted(new
    {
        jobId = newJobId,
        statusUrl = $"/api/transcripts/{newJobId}"
    });
}

[HttpGet("{jobId:guid}")]
public IActionResult GetStatus(Guid jobId)
{
    if (!_store.TryGet(jobId, out var status) || status is null)
    {
        return NotFound();
    }

    return Ok(status);
}


}

