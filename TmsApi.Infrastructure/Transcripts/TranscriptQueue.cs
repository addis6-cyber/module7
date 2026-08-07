using System.Threading.Channels;

namespace TmsApi.Infrastructure.Transcripts;

public sealed class TranscriptQueue
{
    private readonly Channel<Guid> _channel =
        Channel.CreateUnbounded<Guid>();

    public ValueTask QueueAsync(Guid jobId)
        => _channel.Writer.WriteAsync(jobId);

    public ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken)
        => _channel.Reader.ReadAsync(cancellationToken);
}