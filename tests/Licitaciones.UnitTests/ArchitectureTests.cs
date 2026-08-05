using System.Reflection;

namespace Licitaciones.UnitTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void DomainAssemblyDoesNotReferenceInfrastructureWebOrApi()
    {
        var domainAssembly = Assembly.Load("Licitaciones.Domain");
        var referencedAssemblies = domainAssembly.GetReferencedAssemblies().Select(assembly => assembly.Name).ToArray();

        Assert.DoesNotContain(referencedAssemblies, name => string.Equals(name, "Licitaciones.Infrastructure", StringComparison.Ordinal));
        Assert.DoesNotContain(referencedAssemblies, name => string.Equals(name, "Licitaciones.Web", StringComparison.Ordinal));
        Assert.DoesNotContain(referencedAssemblies, name => string.Equals(name, "Licitaciones.Api", StringComparison.Ordinal));
    }
}