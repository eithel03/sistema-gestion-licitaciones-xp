using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace Licitaciones.E2ETests;

public abstract class E2ETestBase(E2EHostFixture host) : PageTest
{
    protected E2EHostFixture Host { get; } = host;

    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = Host.BaseUrl,
        Locale = "es-CR"
    };

    protected Task ResetAsync() => Host.ResetDatabaseAsync();
}
