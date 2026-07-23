using Microsoft.EntityFrameworkCore;
using TmsApi.Application.Dtos;
using TmsApi.Application.Interfaces;
using TmsApi.Infrastructure.Data;

namespace TmsApi.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly TmsDbContext _context;

    public StudentRepository(TmsDbContext context)
    {
        _context = context;
    }

   public async Task<StudentResponseDto?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken)
{
    return await _context.Students
        .Where(s => s.Id == id)
        .Select(s => new StudentResponseDto
        {
            Id = s.Id,
            RegistrationNumber = s.RegistrationNumber,
            Name = s.Name,
            GPA = s.GPA,
            IsActive = s.IsActive
        })
        .FirstOrDefaultAsync(cancellationToken);
}
}