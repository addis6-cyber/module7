using TmsApi.Data;
using TmsApi.Entities;

namespace TmsApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly TmsDbContext _context;

    public StudentRepository(TmsDbContext context)
    {
        _context = context;
    }

    public List<Student> GetAll()
    {
        return _context.Students.ToList();
    }
}