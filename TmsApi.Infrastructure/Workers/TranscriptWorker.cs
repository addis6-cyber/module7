using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TmsApi.Application.Transcripts;
using TmsApi.Infrastructure.Transcripts;

namespace TmsApi.Infrastructure.Workers;

public sealed class TranscriptWorker : BackgroundService
{
    private readonly ChannelReader<TranscriptJobRequest> _reader;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ITranscriptStatusStore _statusStore;
    private readonly ILogger<TranscriptWorker> _logger;

    public TranscriptWorker(
        Channel<TranscriptJobRequest> channel,
        IServiceScopeFactory scopeFactory,
        ITranscriptStatusStore statusStore,
        ILogger<TranscriptWorker> logger)
    {
        _reader = channel.Reader;
        _scopeFactory = scopeFactory;
        _statusStore = statusStore;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in _reader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation(
                "Transcript worker started job {JobId} for student {StudentId}",
                job.JobId,
                job.StudentId);

            _statusStore.MarkProcessing(job.JobId);

            try
            {
                using var scope = _scopeFactory.CreateScope();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                var fileName = $"transcript-{job.StudentId}.pdf";

                _statusStore.MarkCompleted(job.JobId, fileName);

                _logger.LogInformation(
                    "Transcript worker completed job {JobId}",
                    job.JobId);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning(
                    "Transcript worker canceled job {JobId}",
                    job.JobId);

                _statusStore.MarkFailed(job.JobId, "Canceled");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Transcript worker failed job {JobId}",
                    job.JobId);

                _statusStore.MarkFailed(job.JobId, ex.Message);
            }
        }
    }
}