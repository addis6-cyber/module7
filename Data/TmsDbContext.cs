using Microsoft.EntityFrameworkCore;
using TmsApi.Domain.Entities;
namespace TmsApi.Data;

public class TmsDbContext(DbContextOptions<TmsDbContext> options) : DbContext(options)
{
public DbSet<Student> Students => Set<Student>();
public DbSet<Course> Courses => Set<Course>();
public DbSet<Enrollment> Enrollments => Set<Enrollment>();

 protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TmsDbContext).Assembly);

         base.OnModelCreating(modelBuilder);

        //Exercise 9-Part B Global Query Filter (Soft Delete)
         modelBuilder.Entity<Student>()
            .HasQueryFilter(s => s.IsActive);
    }
}
