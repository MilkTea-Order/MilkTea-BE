using MilkTea.Domain.SharedKernel.Exceptions;

namespace MilkTea.Domain.Catalog.Material.Exceptions
{
    public sealed class MaterialUnitMinException : DomainException
    {
        public MaterialUnitMinException() : base("Material unit min is required.") { }
    }
    public sealed class MaterialUnitMaxException : DomainException
    {
        public MaterialUnitMaxException() : base("Material unit max is required.") { }
    }
    public sealed class MaterialNameException : DomainException
    {
        public MaterialNameException() : base("Material name min is required.") { }
    }
    public sealed class MaterialGroupException : DomainException
    {
        public MaterialGroupException() : base("Material group is required.") { }
    }

    public sealed class MaterialStyleQuantityException : DomainException
    {
        public MaterialStyleQuantityException() : base("Material style quantity is required.") { }
    }
}
