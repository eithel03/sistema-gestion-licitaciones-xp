using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Licitaciones.IntegrationTests.Persistence;

public sealed class PersistenceConventionsTests
{
    [Fact]
    public void ApplyPersistenceConventionsConfiguresOnlyReusablePersistenceProperties()
    {
        var options = new DbContextOptionsBuilder<ConventionTestDbContext>()
            .UseNpgsql("Host=localhost;Database=unused;Username=unused;Password=unused")
            .Options;

        using var context = new ConventionTestDbContext(options);

        var entityType = context.Model.FindEntityType(typeof(ConventionEntity));

        Assert.NotNull(entityType);
        Assert.Equal("timestamp with time zone", entityType.FindProperty(nameof(ConventionEntity.CreatedAt))?.GetColumnType());
        Assert.Equal("timestamp with time zone", entityType.FindProperty(nameof(ConventionEntity.UpdatedAt))?.GetColumnType());
        Assert.Equal("timestamp with time zone", entityType.FindProperty(nameof(ConventionEntity.DeletedAt))?.GetColumnType());
        Assert.True(entityType.FindProperty(nameof(ConventionEntity.Version))?.IsConcurrencyToken);
        Assert.Equal(18, entityType.FindProperty(nameof(ConventionEntity.Amount))?.GetPrecision());
        Assert.Equal(2, entityType.FindProperty(nameof(ConventionEntity.Amount))?.GetScale());
        Assert.Null(entityType.FindProperty(nameof(ConventionEntity.Ratio))?.GetPrecision());
        Assert.Null(entityType.FindProperty(nameof(ConventionEntity.Ratio))?.GetScale());
    }

    private sealed class ConventionTestDbContext : DbContext
    {
        public ConventionTestDbContext(DbContextOptions<ConventionTestDbContext> options)
            : base(options)
        {
        }

        public DbSet<ConventionEntity> Entities => Set<ConventionEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConventionEntity>(builder =>
            {
                builder.HasKey(entity => entity.Id);
                builder.Property(entity => entity.Amount).HasMoneyPrecision();
            });

            modelBuilder.ApplyPersistenceConventions();
        }
    }

    private sealed class ConventionEntity
    {
        public int Id { get; init; }

        public DateTimeOffset CreatedAt { get; init; }

        public DateTimeOffset UpdatedAt { get; init; }

        public DateTimeOffset? DeletedAt { get; init; }

        public uint Version { get; init; }

        public decimal Amount { get; init; }

        public decimal Ratio { get; init; }
    }
}
