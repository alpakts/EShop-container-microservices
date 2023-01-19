using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infastructure.EntitityConfigurations
{
    public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).UseHiLo("catalog_tyoe_hilo").IsRequired();

            builder.Property(p => p.Type).IsRequired().HasMaxLength(128);
        }
    }
}
