using FluentValidation;

namespace TmsApi.Application.Students.Commands;

public sealed class CreateStudentCommandValidator
    : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GPA)
            .InclusiveBetween(0, 4);

        RuleFor(x => x.IsActive)
            .NotNull();
    }
}