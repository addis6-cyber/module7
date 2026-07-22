using MediatR;
using TmsApi.Application.Dtos;

namespace TmsApi.Application.Courses.Commands;

public sealed record CreateCourseCommand(
    string Code,
    string Title,
    int MaxCapacity
) : IRequest<CourseResponseDto>;