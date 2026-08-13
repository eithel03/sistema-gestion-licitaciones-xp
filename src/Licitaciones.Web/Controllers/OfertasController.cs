using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Ofertas;
using Licitaciones.Application.Proveedores;
using Licitaciones.Web.Models.Ofertas;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class OfertasController : Controller
{
    private readonly IOfertaService _service;
    private readonly ILicitacionService _licitaciones;
    private readonly IProveedorService _proveedores;

    public OfertasController(IOfertaService service, ILicitacionService licitaciones, IProveedorService proveedores)
    {
        _service = service;
        _licitaciones = licitaciones;
        _proveedores = proveedores;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, Guid? licitacionId = null, Guid? proveedorId = null, string sort = "registered", CancellationToken cancellationToken = default)
    {
        var offers = await _service.ListAsync(new OfertaQuery(page, pageSize, licitacionId, proveedorId, sort), cancellationToken);
        var selections = await LoadSelectionsAsync(cancellationToken);
        return View(new OfertaIndexViewModel(offers.Value!, licitacionId, proveedorId, sort, selections.Licitaciones, selections.Proveedores));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.Succeeded) return NotFound();
        var licitacion = await _licitaciones.GetByIdAsync(result.Value!.LicitacionId, cancellationToken);
        var proveedor = await _proveedores.GetByIdAsync(result.Value.ProveedorId, cancellationToken);
        return View(new OfertaDetailsViewModel(result.Value, licitacion.Value, proveedor.Value));
    }

    public async Task<IActionResult> Create(Guid? licitacionId = null, Guid? proveedorId = null, CancellationToken cancellationToken = default)
    {
        var model = new OfertaFormViewModel { LicitacionId = licitacionId, ProveedorId = proveedorId };
        await PopulateSelectionsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OfertaFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.CreateAsync(new CrearOfertaRequest(model.LicitacionId!.Value, model.ProveedorId!.Value, model.MontoOfertadoCrc), cancellationToken);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Oferta creada correctamente.";
                return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
            }
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible crear la oferta.");
        }
        await PopulateSelectionsAsync(model, cancellationToken);
        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.Succeeded) return NotFound();
        return View(new OfertaFormViewModel
        {
            LicitacionId = result.Value!.LicitacionId,
            ProveedorId = result.Value.ProveedorId,
            MontoOfertadoCrc = result.Value.MontoOfertadoCrc,
            Version = result.Value.Version
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, OfertaFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.UpdateAsync(id, new ActualizarOfertaRequest(model.MontoOfertadoCrc, model.Version), cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == OfertaResultStatus.NotFound) return NotFound();
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible editar la oferta.");
            return View(model);
        }
        TempData["SuccessMessage"] = "Oferta actualizada correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? View(result.Value) : NotFound();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == OfertaResultStatus.NotFound) return NotFound();
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Details), new { id });
        }
        TempData["SuccessMessage"] = "Oferta eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateSelectionsAsync(OfertaFormViewModel model, CancellationToken cancellationToken)
    {
        var values = await LoadSelectionsAsync(cancellationToken);
        model.Licitaciones = values.Licitaciones;
        model.Proveedores = values.Proveedores;
    }

    private async Task<(IReadOnlyList<LicitacionResponse> Licitaciones, IReadOnlyList<ProveedorResponse> Proveedores)> LoadSelectionsAsync(CancellationToken cancellationToken)
    {
        var licitaciones = await _licitaciones.ListAsync(new LicitacionQuery(PageSize: 100), cancellationToken);
        var proveedores = await _proveedores.ListAsync(new ProveedorQuery(PageSize: 100), cancellationToken);
        return (licitaciones.Value!.Items, proveedores.Value!.Items);
    }
}
