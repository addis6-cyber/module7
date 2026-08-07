namespace TmsApi.Application.Transcripts;

public sealed class IdempotencyRecord
{
    public string Key { get; init; } = string.Empty;

    public Guid JobId { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }
        = DateTimeOffset.UtcNow;
}