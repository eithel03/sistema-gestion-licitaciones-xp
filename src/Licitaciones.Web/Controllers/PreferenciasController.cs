using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class PreferenciasController : Controller
{
    private const string CurrencyCookie = "licitaciones.currency";
    private const string ThemeCookie = "licitaciones.theme";

    [HttpPost]
    public IActionResult Moneda(string moneda, string? returnUrl = null)
    {
        var value = string.Equals(moneda, "USD", StringComparison.OrdinalIgnoreCase) ? "USD" : "CRC";
        WriteCookie(CurrencyCookie, value);
        return RedirectToLocal(returnUrl);
    }

    [HttpPost]
    public IActionResult Tema(string tema, string? returnUrl = null)
    {
        var value = string.Equals(tema, "dark", StringComparison.OrdinalIgnoreCase) ? "dark" : "light";
        WriteCookie(ThemeCookie, value);
        return RedirectToLocal(returnUrl);
    }

    private void WriteCookie(string name, string value) =>
        Response.Cookies.Append(name, value, new CookieOptions
        {
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddYears(1)
        });

    private IActionResult RedirectToLocal(string? returnUrl) =>
        !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Home");
}
