using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Proveedores;

namespace Licitaciones.Web.Models.Proveedores;

public sealed record ProveedorDetailsViewModel(ProveedorResponse Proveedor, IReadOnlyList<OfertaResponse> Ofertas);
