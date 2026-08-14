using Licitaciones.Domain.TiposCambio;
using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Configurations;

public sealed class TipoCambioConfiguration : IEntityTypeConfiguration<TipoCambio>
{
    public void Configure(EntityTypeBuilder<TipoCambio> builder)
    {
        builder.ToTable("TiposCambio", table =>
        {
            table.HasCheckConstraint("CK_TiposCambio_CrcPorUsdPositivo", "\"CrcPorUsd\" > 0");
        });
        builder.HasKey(tipoCambio => tipoCambio.Id);
        builder.Property(tipoCambio => tipoCambio.Id).ValueGeneratedNever();
        builder.Property(tipoCambio => tipoCambio.Fecha).IsRequired();
        builder.Property(tipoCambio => tipoCambio.CrcPorUsd).HasMoneyPrecision().IsRequired();
        builder.Property(tipoCambio => tipoCambio.Activo).IsRequired();
        builder.Property(tipoCambio => tipoCambio.Version).IsRowVersion();
        builder.HasIndex(tipoCambio => tipoCambio.Fecha)
            .HasDatabaseName("IX_TiposCambio_Fecha");
        builder.HasIndex(tipoCambio => tipoCambio.Activo)
            .IsUnique()
            .HasFilter("\"Activo\" = TRUE")
            .HasDatabaseName("IX_TiposCambio_UnicoActivo");
    }
}
