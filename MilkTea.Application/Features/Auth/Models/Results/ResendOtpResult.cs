using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results;

public class ResendOtpResult
{
    public StringListEntry ResultData { get; set; } = new();
}
