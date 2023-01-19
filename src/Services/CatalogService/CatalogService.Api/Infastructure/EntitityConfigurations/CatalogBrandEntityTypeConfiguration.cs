using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Api.Infastructure.EntitityConfigurations
{
    public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).UseHiLo("catalog_brand_hilo").IsRequired();

            builder.Property(p => p.Brand).IsRequired().HasMaxLength(128);
        }
    }
}
