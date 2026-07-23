using MediatR;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;
using TmsApi.Domain.Entities;

namespace TmsApi.Application.Students.Commands;

public sealed class CreateStudentCommandHandler
    : IRequestHandler<CreateStudentCommand, StudentResponseDto>
{
    private readonly IStudentRepository _repository;

    public CreateStudentCommandHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentResponseDto> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = new Student
        {
            RegistrationNumber = request.RegistrationNumber,
            Name = request.Name,
            GPA = request.GPA,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(student, cancellationToken);

        return new StudentResponseDto
        {
            Id = student.Id,
            RegistrationNumber = student.RegistrationNumber,
            Name = student.Name,
            GPA = student.GPA,
            IsActive = student.IsActive
        };
    }
}