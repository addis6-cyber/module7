//module6 exercise4
// Create the Enrollment Service Interface
using TmsApi.Dtos;

namespace TmsApi.Services;

public interface IEnrollmentService
{
    Task<bool> EnrollStudentAsync(
        int courseId,
        CreateEnrollmentRequest request,
        CancellationToken ct);
}