using MediatR;
using TmsApi.Application.Dtos;

namespace TmsApi.Application.Courses.Queries;

public sealed record GetCourseByIdQuery(
    int Id
) : IRequest<CourseResponseDto?>;