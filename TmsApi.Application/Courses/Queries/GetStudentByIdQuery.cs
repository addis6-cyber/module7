using MediatR;
using TmsApi.Application.Dtos;
namespace TmsApi.Application.Students.Queries;

public sealed record GetStudentByIdQuery(int Id)
    : IRequest<StudentResponseDto?>;