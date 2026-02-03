using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Configuration.Entities;

namespace MilkTea.Infrastructure.Persistence.Configurations.Configuration;

public class DefinitionConfiguration : IEntityTypeConfiguration<Definition>
{
    public void Configure(EntityTypeBuilder<Definition> builder)
    {
        builder.ToTable("Definition");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");

        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.Value).HasColumnName("Value");
        builder.Property(x => x.ValueImage).HasColumnName("ValueImage");
        builder.Property(x => x.IsEdit).HasColumnName("IsEdit").IsRequired();
        builder.Property(x => x.IsEncrypt).HasColumnName("IsEncrypt").IsRequired();
        builder.Property(x => x.DefinitionGroupID).HasColumnName("DefinitionGroupID").IsRequired();

        builder.Ignore(x => x.CreatedDate);
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.UpdatedDate);
        builder.Ignore(x => x.UpdatedBy);


    }
}
