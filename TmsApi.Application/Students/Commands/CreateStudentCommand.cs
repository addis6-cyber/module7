using MediatR;
using TmsApi.Application.Dtos;

namespace TmsApi.Application.Students.Commands;

public sealed record CreateStudentCommand(
    string RegistrationNumber,
    string Name,
    decimal GPA,
    bool IsActive
) : IRequest<StudentResponseDto>;