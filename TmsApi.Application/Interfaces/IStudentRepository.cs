using TmsApi.Application.Dtos;
using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface IStudentRepository
{
    Task<StudentResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Student student,
        CancellationToken cancellationToken);
}