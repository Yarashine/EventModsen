namespace EventModsen.Validators;

using Application.DTOs.RequestDto;
using FluentValidation;
using System;

public class UpdateEventDtoValidator : AbstractValidator<UpdateEventDto>
{
    public UpdateEventDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(-1).WithMessage("Id должен быть больше или равно 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов");

        RuleFor(x => x.DateTimeEvent)
            .GreaterThan(DateTime.UtcNow).WithMessage("Дата события не может быть в прошлом");

        RuleFor(x => x.Location)
            .MaximumLength(200).WithMessage("Локация не должна превышать 200 символов");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Категория не должна превышать 50 символов");

        RuleFor(x => x.MaxCountMembers)
            .GreaterThan(0).WithMessage("Количество участников должно быть больше 0");
    }
}

