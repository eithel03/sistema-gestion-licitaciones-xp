using System.Reflection;

namespace Licitaciones.IntegrationTests;

public sealed class InfrastructureAssemblyTests
{
    [Fact]
    public void InfrastructureAssemblyExposesAddInfrastructureExtension()
    {
        var infrastructureType = typeof(Licitaciones.Infrastructure.DependencyInjection);
        var method = infrastructureType.GetMethod(
            "AddInfrastructure",
            BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(method);
    }
}