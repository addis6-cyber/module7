using MediatR;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Queries;

public sealed class GetCourseByIdQueryHandler
    : IRequestHandler<GetCourseByIdQuery, CourseResponseDto?>
{
    private readonly ICourseService _courseService;

    public GetCourseByIdQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<CourseResponseDto?> Handle(
        GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _courseService.GetByIdAsync(
            request.Id,
            cancellationToken);
    }
}