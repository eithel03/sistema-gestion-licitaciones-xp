using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Conventions;

public static class MoneyPropertyBuilderExtensions
{
    public static PropertyBuilder<decimal> HasMoneyPrecision(this PropertyBuilder<decimal> propertyBuilder)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilder);

        return propertyBuilder.HasPrecision(18, 2);
    }

    public static PropertyBuilder<decimal?> HasMoneyPrecision(this PropertyBuilder<decimal?> propertyBuilder)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilder);

        return propertyBuilder.HasPrecision(18, 2);
    }
}
