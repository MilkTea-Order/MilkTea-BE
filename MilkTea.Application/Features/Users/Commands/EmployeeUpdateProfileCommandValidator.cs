using FluentValidation;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class EmployeeUpdateProfileCommandValidator : AbstractValidator<EmployeeUpdateProfileCommand>
{
    public EmployeeUpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.FullName))
            .WithMessage("FullName không được để trống");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email không hợp lệ");

        RuleFor(x => x.GenderID)
            .GreaterThan(0)
            .When(x => x.GenderID.HasValue)
            .WithMessage("GenderID phải lớn hơn 0");

        RuleFor(x => x.BirthDay)
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.BirthDay.HasValue)
            .WithMessage("BirthDay không được lớn hơn ngày hiện tại");
    }
}
