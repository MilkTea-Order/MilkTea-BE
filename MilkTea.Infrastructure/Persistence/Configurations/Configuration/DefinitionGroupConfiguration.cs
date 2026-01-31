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
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();

        builder.HasMany(x => x.Denifitions)
        .WithOne()
        .HasForeignKey("DefinitionGroupID")
        .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Denifitions)
                .HasField("_vDenifitions")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
