using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MilkTea.Domain.Entities.Config;

namespace MilkTea.Infrastructure.Persistence.Configurations.Config
{
    public class DefinitionGroupConfiguration : IEntityTypeConfiguration<DefinitionGroup>
    {
        public void Configure(EntityTypeBuilder<DefinitionGroup> builder)
        {
            builder.ToTable("definitiongroup");

            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        }
    }
}
