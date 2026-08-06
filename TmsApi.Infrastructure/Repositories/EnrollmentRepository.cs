using Microsoft.EntityFrameworkCore;
using TmsApi.Application.Interfaces;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Data;

namespace TmsApi.Infrastructure.Repositories;

public sealed class EnrollmentRepository : IEnrollmentRepository
{
    private readonly TmsDbContext _context;

    public EnrollmentRepository(TmsDbContext context)
    {
        _context = context;
    }

    public Task<bool> StudentExistsAsync(
        int studentId,
        CancellationToken cancellationToken)
    {
        return _context.Students
            .AnyAsync(s => s.Id == studentId, cancellationToken);
    }

    public Task<Course?> GetCourseByCodeAsync(
        string courseCode,
        CancellationToken cancellationToken)
    {
        return _context.Courses
            .FirstOrDefaultAsync(c => c.Code == courseCode, cancellationToken);
    }

    public Task<bool> ExistsAsync(
        int studentId,
        int courseId,
        CancellationToken cancellationToken)
    {
        return _context.Enrollments
            .AnyAsync(
                e => e.StudentId == studentId && e.CourseId == courseId,
                cancellationToken);
    }

    public async Task<Enrollment> AddAsync(
        Enrollment enrollment,
        CancellationToken cancellationToken)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
        return enrollment;
    }
}