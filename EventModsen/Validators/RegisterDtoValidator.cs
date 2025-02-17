namespace EventModsen.Validators;

using Application.DTOs.RequestDto;
using FluentValidation;
using System;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна")
            .MaximumLength(50).WithMessage("Фамилия не должна превышать 50 символов");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Дата рождения обязательна");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Некорректный формат Email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву");
    }
}

