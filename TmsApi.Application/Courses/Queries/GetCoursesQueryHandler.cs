using MediatR;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Queries;

public sealed class GetCoursesQueryHandler
    : IRequestHandler<GetCoursesQuery, PagedResponse<CourseResponseDto>>
{
    private readonly ICourseService _courseService;

    public GetCoursesQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<PagedResponse<CourseResponseDto>> Handle(
        GetCoursesQuery request,
        CancellationToken cancellationToken)
    {
        return await _courseService.GetPagedAsync(
            request.Request,
            cancellationToken);
    }
}