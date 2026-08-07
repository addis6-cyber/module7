namespace TmsApi.Application.Transcripts;
public sealed record GenerateTranscriptRequest( int StudentId, string Format);

public enum TranscriptState
 { 
    Queued, 
    Processing, 
    Completed, 
    Failed 
}
public sealed class TranscriptStatus
{ public Guid JobId { get; init; } 
public int StudentId { get; init; } 
public string Format { get; init; } = string.Empty;

public TranscriptState State { get; set; } = TranscriptState.Queued; 
public string? DownloadUrl { get; set; } 
public string? Error { get; set; }
public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow; 
public DateTimeOffset? CompletedAtUtc { get; set; }
}