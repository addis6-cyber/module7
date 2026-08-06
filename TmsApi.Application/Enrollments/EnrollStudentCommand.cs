using MediatR;
using TmsApi.Application.Common;
using TmsApi.Application.Dtos;

namespace TmsApi.Application.Enrollments.Commands;

public sealed record EnrollStudentCommand(
    int StudentId,
    string CourseCode)
    : IRequest<Result<EnrollmentResponseDto, EnrollmentError>>;