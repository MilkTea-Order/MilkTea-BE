namespace MilkTea.Domain.SharedKernel.Abstractions;


public interface IAuditable
{
    int CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    int? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
}
