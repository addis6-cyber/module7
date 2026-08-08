using Microsoft.AspNetCore.SignalR;
using TmsApi.Application.Transcripts;
using TmsApi.Hubs;

namespace TmsApi.Services;

public sealed class SignalRTranscriptNotificationPublisher
    : ITranscriptNotificationPublisher
{
    private readonly IHubContext<NotificationsHub> _hub;

    public SignalRTranscriptNotificationPublisher(
        IHubContext<NotificationsHub> hub)
    {
        _hub = hub;
    }

    public Task PublishCompletedAsync(
        Guid jobId,
        int studentId,
        string downloadUrl,
        CancellationToken cancellationToken)
    {
        return _hub.Clients.All.SendAsync(
            "TranscriptCompleted",
            new
            {
                jobId,
                studentId,
                downloadUrl
            },
            cancellationToken);
    }
}