using System.ComponentModel.DataAnnotations;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Proveedores;

namespace Licitaciones.Web.Models.Ofertas;

public sealed class OfertaFormViewModel : IValidatableObject
{
    [Display(Name = "Licitacion")]
    [Required(ErrorMessage = "La licitacion es requerida.")]
    public Guid? LicitacionId { get; set; }

    [Display(Name = "Proveedor")]
    [Required(ErrorMessage = "El proveedor es requerido.")]
    public Guid? ProveedorId { get; set; }

    [Display(Name = "Monto ofertado CRC")]
    public decimal MontoOfertadoCrc { get; set; }

    public uint? Version { get; set; }
    public IReadOnlyList<LicitacionResponse> Licitaciones { get; set; } = [];
    public IReadOnlyList<ProveedorResponse> Proveedores { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MontoOfertadoCrc <= 0m)
        {
            yield return new ValidationResult("El monto ofertado debe ser mayor que cero.", [nameof(MontoOfertadoCrc)]);
        }
    }
}

public sealed record OfertaIndexViewModel(
    OfertaPage Page,
    Guid? LicitacionId,
    Guid? ProveedorId,
    string Sort,
    IReadOnlyList<LicitacionResponse> Licitaciones,
    IReadOnlyList<ProveedorResponse> Proveedores);

public sealed record OfertaDetailsViewModel(OfertaResponse Oferta, LicitacionResponse? Licitacion, ProveedorResponse? Proveedor);
