using Licitaciones.Application.Licitaciones;
using Licitaciones.Web.Models.Licitaciones;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class LicitacionesController : Controller
{
    private readonly ILicitacionService _service;
    private static readonly TimeZoneInfo CostaRicaTimeZone = ResolveCostaRicaTimeZone();

    public LicitacionesController(ILicitacionService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null, string sort = "code", CancellationToken cancellationToken = default)
    {
        var result = await _service.ListAsync(new LicitacionQuery(page, pageSize, search, sort), cancellationToken);
        var p = result.Value!;
        return View(new LicitacionIndexViewModel(p.Items, p.Page, p.PageSize, p.TotalItems, p.TotalPages, search, sort));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? View(result.Value) : NotFound();
    }

    public IActionResult Create() => View(new LicitacionFormViewModel { FechaCierreLocal = ToCostaRica(DateTimeOffset.UtcNow.AddDays(7)).DateTime });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LicitacionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.CreateAsync(new CrearLicitacionRequest(model.Codigo, model.Titulo, model.PresupuestoCrc, ToUtc(model.FechaCierreLocal)), cancellationToken);
        if (!result.Succeeded)
        {
            AddModelError(result);
            return View(model);
        }

        TempData["SuccessMessage"] = "Licitacion creada correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.Succeeded) return NotFound();
        ViewData["LicitacionId"] = id;
        return View(ToForm(result.Value!));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, LicitacionFormViewModel model, CancellationToken cancellationToken)
    {
        ViewData["LicitacionId"] = id;
        if (!ModelState.IsValid) return View(model);
        var result = await _service.UpdateAsync(id, new ActualizarLicitacionRequest(model.Codigo, model.Titulo, model.PresupuestoCrc, ToUtc(model.FechaCierreLocal), model.Version), cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == LicitacionResultStatus.NotFound) return NotFound();
            AddModelError(result);
            return View(model);
        }

        TempData["SuccessMessage"] = "Licitacion actualizada correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded) return result.Status == LicitacionResultStatus.NotFound ? NotFound() : RedirectToAction(nameof(Details), new { id });
        TempData["SuccessMessage"] = "Licitacion retirada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.PublishAsync(id, cancellationToken);
        if (!result.Succeeded) return ToRedirectError(id, result);
        TempData["SuccessMessage"] = "Licitacion publicada correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.CloseAsync(id, cancellationToken);
        if (!result.Succeeded) return ToRedirectError(id, result);
        TempData["SuccessMessage"] = "Licitacion cerrada correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private IActionResult ToRedirectError<T>(Guid id, LicitacionResult<T> result)
    {
        if (result.Status == LicitacionResultStatus.NotFound) return NotFound();
        TempData["ErrorMessage"] = result.ErrorMessage ?? "No fue posible completar la operacion.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private static LicitacionFormViewModel ToForm(LicitacionResponse response)
    {
        return new LicitacionFormViewModel
        {
            Codigo = response.Codigo,
            Titulo = response.Titulo,
            PresupuestoCrc = response.PresupuestoCrc,
            FechaCierreLocal = ToCostaRica(response.FechaCierreUtc).DateTime,
            Version = response.Version
        };
    }

    private static DateTimeOffset ToCostaRica(DateTimeOffset utc) => TimeZoneInfo.ConvertTime(utc, CostaRicaTimeZone);

    private static DateTimeOffset ToUtc(DateTime local)
    {
        var unspecified = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
        return new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(unspecified, CostaRicaTimeZone));
    }

    private static TimeZoneInfo ResolveCostaRicaTimeZone()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"); }
        catch (TimeZoneNotFoundException) { return TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica"); }
    }

    private void AddModelError<T>(LicitacionResult<T> result) =>
        ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible guardar la licitacion.");
}
