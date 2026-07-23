/*using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface IStudentRepository
{
    List<Student> GetAll();

    List<Student> GetActive();

    List<Student> GetByGpa();

    Student? GetByRegistrationNumber(string registrationNumber);

    void Add(Student student);

    void SaveChanges();
}*/
using TmsApi.Application.Dtos;

namespace TmsApi.Application.Interfaces;

public interface IStudentRepository
{
    Task<StudentResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);
}