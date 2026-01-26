using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Configuration.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Configuration;

public class DefinitionConfiguration : IEntityTypeConfiguration<Definition>
{
    public void Configure(EntityTypeBuilder<Definition> builder)
    {
        builder.ToTable("definition");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.Value).HasColumnName("Value");
        builder.Property(x => x.ValueImage).HasColumnName("ValueImage");
        builder.Property(x => x.IsEdit).HasColumnName("IsEdit").IsRequired();
        builder.Property(x => x.IsEncrypt).HasColumnName("IsEncrypt").IsRequired();
        builder.Property(x => x.DefinitionGroupID).HasColumnName("DefinitionGroupID").IsRequired();

        builder.HasOne(d => d.DefinitionGroup)
            .WithMany(dg => dg.Definitions)
            .HasForeignKey(d => d.DefinitionGroupID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
