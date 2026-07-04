using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Dtos;
using TmsApi.Entities;

namespace TmsApi.Services;

public class CourseService : ICourseService
{
    private readonly TmsDbContext _context;

    public CourseService(TmsDbContext context)
    {
        _context = context;
    }

    // Get one course by Id
    public async Task<CourseResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                MaxCapacity = c.Capacity,
                EnrollmentCount = c.Enrollments.Count()
            })
            .FirstOrDefaultAsync(ct);
    }

    // Create a new course
    public async Task<CourseResponseDto> CreateAsync(
        CreateCourseRequest request,
        CancellationToken ct)
    {
        var course = new Course
        {
            Code = request.Code,
            Title = request.Title,
            Capacity = request.MaxCapacity
        };

        _context.Courses.Add(course);

        await _context.SaveChangesAsync(ct);

        return await GetByIdAsync(course.Id, ct)
            ?? throw new Exception("Course could not be loaded.");
    }

    // Check whether a course code already exists
    public async Task<bool> CodeExistsAsync(
        string code,
        CancellationToken ct)
    {
        return await _context.Courses
            .AsNoTracking()
            .AnyAsync(c => c.Code == code, ct);
    }
}