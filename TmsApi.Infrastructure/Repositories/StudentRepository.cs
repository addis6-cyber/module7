using TmsApi.Application.Interfaces;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TmsApi.Infrastructure.Repositories;

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

    public List<Student> GetActive()
    {
        return _context.Students
            .Where(s => s.IsActive)
            .ToList();
    }

    public List<Student> GetByGpa()
    {
        return _context.Students
            .OrderByDescending(s => s.GPA)
            .ToList();
    }

    public Student? GetByRegistrationNumber(string registrationNumber)
    {
        return _context.Students
            .FirstOrDefault(s => s.RegistrationNumber == registrationNumber);
    }

    public void Add(Student student)
    {
        _context.Students.Add(student);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}