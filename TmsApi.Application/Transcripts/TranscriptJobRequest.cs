namespace TmsApi.Application.Transcripts;

public sealed record TranscriptJobRequest(
    Guid JobId,
    int StudentId,
    string Format);