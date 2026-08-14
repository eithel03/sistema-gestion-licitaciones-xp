using Licitaciones.Domain.Proveedores;

namespace Licitaciones.UnitTests.Domain.Proveedores;

public sealed class ProveedorTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 10, 9, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData("Empresa Central")]
    [InlineData("Empresa 123, S.A.")]
    [InlineData("Proveedor (Norte)")]
    public void CreateAcceptsValidName(string nombre)
    {
        var proveedor = Proveedor.Create(nombre, Now);

        Assert.Equal(nombre, proveedor.Nombre);
        Assert.Equal(Now, proveedor.CreatedAt);
        Assert.Null(proveedor.DeletedAt);
    }

    [Fact]
    public void CreateAcceptsNameWithExactlyTwoHundredCharacters()
    {
        var nombre = new string('A', 200);

        var proveedor = Proveedor.Create(nombre, Now);

        Assert.Equal(nombre, proveedor.Nombre);
    }

    [Fact]
    public void CreateRejectsNameLongerThanTwoHundredCharacters()
    {
        var nombre = new string('A', 201);

        var exception = Assert.Throws<ProveedorValidationException>(() => Proveedor.Create(nombre, Now));

        Assert.Contains(exception.Errors, error => error.Code == ProveedorErrors.NombreLongitudMaxima);
    }
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateRejectsEmptyName(string nombre)
    {
        var exception = Assert.Throws<ProveedorValidationException>(() => Proveedor.Create(nombre, Now));

        Assert.Contains(exception.Errors, error => error.Code == ProveedorErrors.NombreRequerido);
    }

    [Fact]
    public void CreateTrimsLateralSpaces()
    {
        var proveedor = Proveedor.Create("  Empresa Central  ", Now);

        Assert.Equal("Empresa Central", proveedor.Nombre);
    }

    [Fact]
    public void CreateReducesRepeatedSpaces()
    {
        var proveedor = Proveedor.Create("Empresa   Central", Now);

        Assert.Equal("Empresa Central", proveedor.Nombre);
    }

    [Theory]
    [InlineData("Empresa Central", " empresa central ")]
    [InlineData("Empresa Central", "EMPRESA   CENTRAL")]
    public void NormalizedNameIgnoresCaseAndRepeatedSpaces(string left, string right)
    {
        var first = ProveedorNameNormalizer.NormalizeForComparison(left);
        var second = ProveedorNameNormalizer.NormalizeForComparison(right);

        Assert.Equal(first, second);
    }

    [Theory]
    [InlineData("Tecnología Empresarial CR")]
    [InlineData("Compañía Nacional 2026")]
    [InlineData("Empresa Ñandú")]
    [InlineData("Servicios Técnicos, S.A.")]
    [InlineData("Soluciones (Costa Rica)")]
    public void CreateAcceptsUnicodeLetters(string nombre)
    {
        Assert.Equal(nombre, Proveedor.Create(nombre, Now).Nombre);
    }

    [Fact]
    public void NormalizedNameUsesUnicodeNormalization()
    {
        var composed = ProveedorNameNormalizer.NormalizeForComparison("Café Central");
        var decomposed = ProveedorNameNormalizer.NormalizeForComparison("Cafe\u0301 Central");

        Assert.Equal(composed, decomposed);
    }

    [Theory]
    [InlineData("Empresa Central")]
    [InlineData("Empresa 123")]
    [InlineData("Empresa, S.A.")]
    [InlineData("Empresa (Central)")]
    public void CreateAcceptsAllowedCharacters(string nombre)
    {
        var proveedor = Proveedor.Create(nombre, Now);

        Assert.Equal(nombre, proveedor.Nombre);
    }

    [Theory]
    [InlineData("Empresa-Central")]
    [InlineData("Empresa/Central")]
    [InlineData("Empresa @ Central")]
    [InlineData("Empresa @ CR")]
    [InlineData("Proveedor #1")]
    [InlineData("Empresa / Servicios")]
    [InlineData("Proveedor & Asociados")]
    [InlineData("Empresa Central!")]
    public void CreateRejectsDisallowedCharacters(string nombre)
    {
        var exception = Assert.Throws<ProveedorValidationException>(() => Proveedor.Create(nombre, Now));

        Assert.Contains(exception.Errors, error => error.Code == ProveedorErrors.NombreCaracteresInvalidos);
    }

    [Fact]
    public void RenameUpdatesNameAndTimestamp()
    {
        var proveedor = Proveedor.Create("Empresa Central", Now);
        var updatedAt = Now.AddHours(1);

        proveedor.Rename("Empresa Nacional", updatedAt);

        Assert.Equal("Empresa Nacional", proveedor.Nombre);
        Assert.Equal(updatedAt, proveedor.UpdatedAt);
    }

    [Fact]
    public void RetireMarksProviderAsDeleted()
    {
        var proveedor = Proveedor.Create("Empresa Central", Now);
        var deletedAt = Now.AddHours(2);

        proveedor.Retire(deletedAt);

        Assert.Equal(deletedAt, proveedor.DeletedAt);
        Assert.Equal(deletedAt, proveedor.UpdatedAt);
    }
}
