using TmsApi.Entities;

namespace TmsApi.Repositories;

public interface IStudentRepository
{
    List<Student> GetAll();
}