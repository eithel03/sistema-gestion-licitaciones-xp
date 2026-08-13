using System.ComponentModel.DataAnnotations;
using Licitaciones.Application.Aprobaciones;

namespace Licitaciones.Web.Models.Aprobaciones;

public sealed class NivelAprobacionFormViewModel : IValidatableObject
{
    [Display(Name = "Monto minimo CRC")]
    public decimal MontoMinimoCrc { get; set; }

    [Display(Name = "Monto maximo CRC")]
    public decimal? MontoMaximoCrc { get; set; }

    [Display(Name = "Aprobador")]
    [Required(ErrorMessage = "El aprobador es requerido.")]
    [StringLength(200, ErrorMessage = "El aprobador no debe superar 200 caracteres.")]
    public string? Aprobador { get; set; }

    public uint? Version { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MontoMinimoCrc <= 0m)
        {
            yield return new ValidationResult("El monto minimo debe ser mayor que cero.", [nameof(MontoMinimoCrc)]);
        }
        if (MontoMaximoCrc.HasValue && MontoMaximoCrc.Value < MontoMinimoCrc)
        {
            yield return new ValidationResult("El monto maximo no puede ser menor que el monto minimo.", [nameof(MontoMaximoCrc)]);
        }
    }
}

public sealed record NivelAprobacionIndexViewModel(NivelAprobacionPage Page);
