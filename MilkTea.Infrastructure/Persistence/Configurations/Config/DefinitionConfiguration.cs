using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Config;

namespace MilkTea.Infrastructure.Persistence.Configurations.Config
{
    public class DefinitionConfiguration : IEntityTypeConfiguration<Definition>
    {
        public void Configure(EntityTypeBuilder<Definition> builder)
        {
            builder.ToTable("definition");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
            builder.Property(x => x.Value).HasColumnName("Value");
            builder.Property(x => x.ValueImage).HasColumnName("ValueImage");
            builder.Property(x => x.IsEdit).HasColumnName("IsEdit").IsRequired();
            builder.Property(x => x.IsEncrypt).HasColumnName("IsEncrypt").IsRequired();
            builder.Property(x => x.DefinitionGroupID).HasColumnName("DefinitionGroupID").IsRequired();

            // Relationships
            builder.HasOne<DefinitionGroup>(d => d.DefinitionGroup)
                .WithMany()
                .HasForeignKey(d => d.DefinitionGroupID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
