using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Configuration.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Configuration;

public class DefinitionGroupConfiguration : IEntityTypeConfiguration<DefinitionGroup>
{
    public void Configure(EntityTypeBuilder<DefinitionGroup> builder)
    {
        builder.ToTable("definitiongroup");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
    }
}
