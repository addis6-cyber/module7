using FluentValidation;

namespace TmsApi.Application.Enrollments.Commands;

public sealed class EnrollStudentCommandValidator
    : AbstractValidator<EnrollStudentCommand>
{
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0);

        RuleFor(x => x.CourseCode)
            .NotEmpty()
            .Matches(@"^[A-Z]{3}-\d{3}$")
            .WithMessage(
                "Course code must follow the format XXX-000 (e.g., CSE-101).");
    }
}