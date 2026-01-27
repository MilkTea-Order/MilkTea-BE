using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class EmployeeUpdateProfileCommand : IRequest<EmployeeUpdateProfileResult>
{
    public string? FullName { get; set; } = null!;
    public int? GenderID { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? IdentityCode { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? CellPhone { get; set; }

    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public IFormFile? BankQRCode { get; set; }
}

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
