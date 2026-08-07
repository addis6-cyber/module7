using TmsApi.Application.Transcripts;

namespace TmsApi.Infrastructure.Transcripts;

public interface ITranscriptStatusStore
{
    TranscriptStatus Create(Guid jobId, int studentId, string format);

    bool TryGet(Guid jobId, out TranscriptStatus? status);

    void MarkProcessing(Guid jobId);

    void MarkCompleted(Guid jobId, string downloadUrl);

    void MarkFailed(Guid jobId, string error);

    bool TryGetJobByKey(string key, out Guid jobId);

    void SaveKey(string key, Guid jobId);
}