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

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TmsDbContext).Assembly);

        modelBuilder.Entity<Student>()
            .HasQueryFilter(s => s.IsActive);

        modelBuilder.Entity<RefreshToken>(entity =>
{
    entity.HasKey(x => x.Id);

    entity.Property(x => x.Token)
        .IsRequired();

    entity.HasIndex(x => x.Token)
        .IsUnique();

    entity.Property(x => x.UserId)
        .IsRequired();

    entity.Property(x => x.ExpiresAt)
        .IsRequired();

    entity.Property(x => x.IsUsed)
        .IsRequired();

    entity.Property(x => x.IsRevoked)
        .IsRequired();
});

    modelBuilder.Entity<Course>()
    .Property(c => c.InstructorId)
    .HasMaxLength(450);

    modelBuilder.Entity<Course>()
    .HasOne<TmsUser>()
    .WithMany()
    .HasForeignKey(c => c.InstructorId)
    .OnDelete(DeleteBehavior.SetNull);
    }
}