namespace Licitaciones.Api;

public static class ApiProblemResults
{
    public static IResult Problem(HttpContext context, int statusCode, string? title, string? detail, string code)
    {
        var extensions = new Dictionary<string, object?>
        {
            ["code"] = code
        };

        if (context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var correlationId) &&
            correlationId is not null)
        {
            extensions["correlationId"] = correlationId.ToString();
        }

        return Results.Problem(
            title: title,
            detail: detail,
            statusCode: statusCode,
            extensions: extensions);
    }
}
