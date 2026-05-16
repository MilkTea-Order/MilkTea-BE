namespace MilkTea.Application.Features.Auth.Models.Dtos
{
    public sealed record PermissionDto(
        int Id,
        string Name,
        string Code,
        List<PermissionDetailDto> PermissionDetails
    );

    public sealed record PermissionDetailDto(
        int Id,
        string Name,
        string Code,
        string? Note
    );
}
