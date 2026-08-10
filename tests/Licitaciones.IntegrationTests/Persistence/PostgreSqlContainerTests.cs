using System.Data;
using Licitaciones.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Licitaciones.IntegrationTests.Persistence;

[Collection(PostgreSqlContainerGroup.Name)]
public sealed class PostgreSqlContainerTests
{
    private readonly PostgreSqlContainerFixture _fixture;

    public PostgreSqlContainerTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PostgreSql16ContainerStartsAndDbContextOpensConnection()
    {
        var options = new DbContextOptionsBuilder<LicitacionesDbContext>()
            .UseNpgsql(_fixture.ConnectionString)
            .Options;

        await using var context = new LicitacionesDbContext(options);

        await context.Database.OpenConnectionAsync();

        Assert.Equal(ConnectionState.Open, context.Database.GetDbConnection().State);

        await using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "select version()";

        var version = await command.ExecuteScalarAsync();

        Assert.IsType<string>(version);
        Assert.Contains("PostgreSQL 16", (string)version);
    }
}

[CollectionDefinition(Name)]
public sealed class PostgreSqlContainerGroup : ICollectionFixture<PostgreSqlContainerFixture>
{
    public const string Name = "PostgreSQL test container";
}

public sealed class PostgreSqlContainerFixture : IAsyncLifetime
{
    static PostgreSqlContainerFixture()
    {
        Environment.SetEnvironmentVariable("TESTCONTAINERS_RYUK_DISABLED", "true");
    }

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("licitaciones_tests")
        .WithUsername("licitaciones_tests")
        .WithPassword("licitaciones_tests")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public Task InitializeAsync()
    {
        return _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}