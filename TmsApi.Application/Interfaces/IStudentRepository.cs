/*using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;
public interface IStudentRepository
{
    List<Student> GetAll();
}*/
using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface IStudentRepository
{
    List<Student> GetAll();

    List<Student> GetActive();

    List<Student> GetByGpa();

    Student? GetByRegistrationNumber(string registrationNumber);

    void Add(Student student);

    void SaveChanges();
}