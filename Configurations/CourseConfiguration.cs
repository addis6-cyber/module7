using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Domain.Entities;

namespace TmsApi.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        // Course Code
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        // Course Title
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        // Capacity
        builder.Property(c => c.Capacity)
            .IsRequired();
    }
}