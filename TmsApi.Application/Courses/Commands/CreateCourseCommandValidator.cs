using FluentValidation;

namespace TmsApi.Application.Courses.Commands;

public sealed class CreateCourseCommandValidator
    : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Matches(@"^[A-Z]{2,3}-\d{3}$")
            .WithMessage("Course code must be like ABC-123.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.MaxCapacity)
            .InclusiveBetween(1, 200);
    }
}