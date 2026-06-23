using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Primary Key
        builder.HasKey(s => s.Id);

        // Registration Number
        builder.Property(s => s.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        // Student Name
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(10);

        // GPA
        builder.Property(s => s.GPA)
            .HasPrecision(3, 2);

        // IsActive
        builder.Property(s => s.IsActive)
            .IsRequired();
    }
}