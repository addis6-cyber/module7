using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Domain.Entities;

namespace TmsApi.Infrastructure.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Grade (optional)
        builder.Property(e => e.Grade)
            .HasMaxLength(2);

        // Enrolled Date
        builder.Property(e => e.EnrolledAt)
            .IsRequired();

        // Relationship: Enrollment -> Student
    builder.HasOne(e => e.Student)
        .WithMany(s => s.Enrollments)
        .HasForeignKey(e => e.StudentId)
        .OnDelete(DeleteBehavior.Restrict);



        // Relationship: Enrollment -> Course
    builder.HasOne(e => e.Course)
        .WithMany(c => c.Enrollments)
        .HasForeignKey(e => e.CourseId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}