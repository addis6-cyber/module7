using System.Collections.Concurrent;
using TmsApi.Application.Transcripts;

namespace TmsApi.Infrastructure.Transcripts;

public sealed class InMemoryTranscriptStatusStore
    : ITranscriptStatusStore
{
    private readonly ConcurrentDictionary<Guid, TranscriptStatus> _jobs = new();
    private readonly ConcurrentDictionary<string, Guid> _keys = new();
    public TranscriptStatus Create(Guid jobId, int studentId, string format)
    {
        var status = new TranscriptStatus
        {
            JobId = jobId,
            StudentId = studentId,
            Format = format,
            State = TranscriptState.Queued
        };

        _jobs[jobId] = status;

    

        return status;
    }
public bool TryGetJobByKey(string key, out Guid jobId)
    => _keys.TryGetValue(key, out jobId);

public void SaveKey(string key, Guid jobId)
    => _keys[key] = jobId;
    public bool TryGet(Guid jobId, out TranscriptStatus? status)
        => _jobs.TryGetValue(jobId, out status);

    public void MarkProcessing(Guid jobId)
    {
        if (_jobs.TryGetValue(jobId, out var status))
        {
            status.State = TranscriptState.Processing;
        }
    }

    public void MarkCompleted(Guid jobId, string downloadUrl)
    {
        if (_jobs.TryGetValue(jobId, out var status))
        {
            status.State = TranscriptState.Completed;
            status.DownloadUrl = downloadUrl;
            status.CompletedAtUtc = DateTimeOffset.UtcNow;
        }
    }

    public void MarkFailed(Guid jobId, string error)
    {
        if (_jobs.TryGetValue(jobId, out var status))
        {
            status.State = TranscriptState.Failed;
            status.Error = error;
        }
    }
}