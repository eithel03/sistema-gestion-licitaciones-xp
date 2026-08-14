using Licitaciones.Application.TiposCambio;
using Licitaciones.Web.Models.TiposCambio;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class TiposCambioController : Controller
{
    private readonly ITipoCambioService _service;

    public TiposCambioController(ITipoCambioService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await _service.ListAsync(new TipoCambioQuery(page, pageSize), cancellationToken);
        return View(new TipoCambioIndexViewModel(result.Value!));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? View(result.Value) : NotFound();
    }

    public IActionResult Create() => View(new TipoCambioFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TipoCambioFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.CreateAsync(new CrearTipoCambioRequest(model.Fecha, model.CrcPorUsd), cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible crear el tipo de cambio.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Tipo de cambio creado correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.Succeeded) return NotFound();
        return View(new TipoCambioFormViewModel
        {
            Fecha = result.Value!.Fecha,
            CrcPorUsd = result.Value.CrcPorUsd,
            Version = result.Value.Version
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TipoCambioFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.UpdateAsync(id, new ActualizarTipoCambioRequest(model.Fecha, model.CrcPorUsd, model.Version), cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == TipoCambioResultStatus.NotFound) return NotFound();
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible editar el tipo de cambio.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Tipo de cambio actualizado correctamente.";
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
            if (result.Status == TipoCambioResultStatus.NotFound) return NotFound();
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["SuccessMessage"] = "Tipo de cambio eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.ActivateAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == TipoCambioResultStatus.NotFound) return NotFound();
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["SuccessMessage"] = "Tipo de cambio activado correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
