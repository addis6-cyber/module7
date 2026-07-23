using MediatR;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;

namespace TmsApi.Application.Students.Queries;

public sealed class GetStudentByIdQueryHandler
    : IRequestHandler<GetStudentByIdQuery, StudentResponseDto?>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentByIdQueryHandler(
        IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentResponseDto?> Handle(
        GetStudentByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _studentRepository.GetByIdAsync(
            request.Id,
            cancellationToken);
    }
}