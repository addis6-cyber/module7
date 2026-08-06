using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface IEnrollmentRepository
{
    Task<bool> StudentExistsAsync(
        int studentId,
        CancellationToken cancellationToken);

    Task<Course?> GetCourseByCodeAsync(
        string courseCode,
        CancellationToken cancellationToken);

    Task<bool> ExistsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken);

    Task<Enrollment> AddAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken);
}