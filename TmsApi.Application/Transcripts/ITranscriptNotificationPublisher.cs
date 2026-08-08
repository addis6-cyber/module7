namespace TmsApi.Application.Transcripts;

public interface ITranscriptNotificationPublisher
{
    Task PublishCompletedAsync(
        Guid jobId,
        int studentId,
        string downloadUrl,
        CancellationToken cancellationToken);
}