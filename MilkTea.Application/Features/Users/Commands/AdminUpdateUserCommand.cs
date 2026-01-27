using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class AdminUpdateUserCommand : IRequest<AdminUpdateUserResult>
{
    public int UserID { get; set; }

    public string? FullName { get; set; }
    public int? GenderID { get; set; }
    public string? BirthDay { get; set; }
    public string? IdentityCode { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? CellPhone { get; set; }


    public int? PositionID { get; set; }
    public DateTime? StartWorkingDate { get; set; }
    public DateTime? EndWorkingDate { get; set; }
    public int? StatusID { get; set; }


    public int? SalaryByHour { get; set; }
    public int? CalcSalaryByMinutes { get; set; }


    public DateTime? ShiftFrom { get; set; }
    public DateTime? ShiftTo { get; set; }
    public bool? IsBreakTime { get; set; }
    public DateTime? BreakTimeFrom { get; set; }
    public DateTime? BreakTimeTo { get; set; }


    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public byte[]? BankQRCode { get; set; }
}

public sealed class AdminUpdateUserCommandValidator : AbstractValidator<AdminUpdateUserCommand>
{
    public AdminUpdateUserCommandValidator()
    {
        RuleFor(x => x.UserID)
            .GreaterThan(0)
            .WithMessage("UserID phải lớn hơn 0");
    }
}
