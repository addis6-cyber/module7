using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Dtos;
using TmsApi.Entities;

namespace TmsApi.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly TmsDbContext _context;

    public EnrollmentService(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EnrollStudentAsync(
        int courseId,
        CreateEnrollmentRequest request,
        CancellationToken ct)
    {
        // Check that the course exists
        var courseExists = await _context.Courses
            .AnyAsync(c => c.Id == courseId, ct);

        if (!courseExists)
            return false;

        // Check that the student exists
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == request.StudentId, ct);

        if (!studentExists)
            return false;

        // Check if already enrolled
        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e =>
                e.CourseId == courseId &&
                e.StudentId == request.StudentId,
                ct);

        if (alreadyEnrolled)
            return false;

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            StudentId = request.StudentId,
            EnrolledAt = DateTime.UtcNow
        };

        using var transaction =
    await _context.Database.BeginTransactionAsync(ct);

try
{
    _context.Enrollments.Add(enrollment);

    await _context.SaveChangesAsync(ct);

    await transaction.CommitAsync(ct);

    return true;
}
catch
{
    await transaction.RollbackAsync(ct);

    return false;
}
}
}