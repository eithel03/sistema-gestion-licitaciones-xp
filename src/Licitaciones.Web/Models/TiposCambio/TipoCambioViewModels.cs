using System.ComponentModel.DataAnnotations;
using Licitaciones.Application.TiposCambio;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Models.TiposCambio;

public sealed class TipoCambioFormViewModel : IValidatableObject
{
    [Display(Name = "Fecha")]
    [DataType(DataType.Date)]
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Display(Name = "CRC por USD")]
    [ModelBinder(BinderType = typeof(FlexibleDecimalModelBinder))]
    public decimal CrcPorUsd { get; set; }

    public uint? Version { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Fecha == default)
        {
            yield return new ValidationResult("La fecha del tipo de cambio es requerida.", [nameof(Fecha)]);
        }

        if (CrcPorUsd <= 0m)
        {
            yield return new ValidationResult("El valor del tipo de cambio debe ser mayor que cero.", [nameof(CrcPorUsd)]);
        }
    }
}

public sealed record TipoCambioIndexViewModel(TipoCambioPage Page);
