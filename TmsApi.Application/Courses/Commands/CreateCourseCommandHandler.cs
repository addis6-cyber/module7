using MediatR;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Courses.Commands;

public sealed class CreateCourseCommandHandler
    : IRequestHandler<CreateCourseCommand, CourseResponseDto>
{
    private readonly ICourseService _courseService;

    public CreateCourseCommandHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<CourseResponseDto> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var createRequest = new CreateCourseRequest
        {
            Code = request.Code,
            Title = request.Title,
            MaxCapacity = request.MaxCapacity
        };

        return await _courseService.CreateAsync(
            createRequest,
            cancellationToken);
    }
}