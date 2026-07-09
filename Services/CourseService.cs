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
    public async Task<PagedResponse<CourseResponseDto>> GetPagedAsync(
    PagedRequest request,
    CancellationToken ct)
{
    var query = _context.Courses.AsQueryable();

    // Search
    if (!string.IsNullOrWhiteSpace(request.Search))
    {
        query = query.Where(c =>
            c.Title.Contains(request.Search) ||
            c.Code.Contains(request.Search));
    }

    // Sorting
    query = request.OrderBy?.ToLower() switch
{
    "code" => request.Descending
        ? query.OrderByDescending(c => c.Code)
        : query.OrderBy(c => c.Code),

    "capacity" => request.Descending
        ? query.OrderByDescending(c => c.Capacity)
        : query.OrderBy(c => c.Capacity),

    _ => request.Descending
        ? query.OrderByDescending(c => c.Title)
        : query.OrderBy(c => c.Title)
};

    // Count total records
    var totalCount = await query.CountAsync(ct);

    // Get one page
    var items = await query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(c => new CourseResponseDto
        {
            Id = c.Id,
            Code = c.Code,
            Title = c.Title,
            MaxCapacity = c.Capacity,
            EnrollmentCount = c.Enrollments.Count()
        })
        .ToListAsync(ct);

    return new PagedResponse<CourseResponseDto>
    {
        Items = items,
        TotalCount = totalCount,
        Page = request.Page,
        PageSize = request.PageSize
    };
}
}