using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TmsApi.Domain.Entities;
using TmsApi.Domain.Users;

namespace TmsApi.Infrastructure.Data;

public class TmsDbContext : IdentityDbContext<TmsUser>
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TmsDbContext).Assembly);

        modelBuilder.Entity<Student>()
            .HasQueryFilter(s => s.IsActive);
    }
}