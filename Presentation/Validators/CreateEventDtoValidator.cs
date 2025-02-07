namespace Presentation.Validators;

using FluentValidation;
using Application.DTOs.RequestDto;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(50).WithMessage("Event name must be less than 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

        RuleFor(x => x.DateTimeEvent)
            .GreaterThan(DateTime.Now).WithMessage("Event date and time must be in the future.");

        RuleFor(x => x.Location)
            .MaximumLength(100).WithMessage("Location must be less than 100 characters.");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Category must be less than 50 characters.");

        RuleFor(x => x.MaxCountMembers)
            .InclusiveBetween(1, 100000).WithMessage("Max number of members must be between 1 and 100000.");
    }
}

