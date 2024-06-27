using Htmx;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();
builder.Services.AddAntiforgery();
var app = builder.Build();

app.UseAntiforgery();

app.MapGet("/", (HttpContext context, [FromServices] IAntiforgery anti) =>
{
    var token = anti.GetAndStoreTokens(context);

    var html = $$"""
        <!DOCTYPE html>
        <html>
            <head>
                <style>
                    li{
                        cursor:pointer;
                    }
                </style>
                <meta name="htmx-config" content='{ "antiForgery": {"headerName" : "{{ token.HeaderName}}", "requestToken" : "{{token.RequestToken }}" } }'>
            </head>
            <body>
            <h1>Asking for prompt</h1>
            <p>Click on the below links to see the response</p>
            <ul hx-prompt="what is your name?">
                <li hx-get="/htmx">GET</li>
                <li hx-post="/htmx">POST</li>
                <li hx-put="/htmx">PUT</li>
                <li hx-patch="/htmx">PATCH</li>
                <li hx-delete="/htmx">DELETE</li>
            </ul>
            <script src="https://unpkg.com/htmx.org@2.0.0" integrity="sha384-wS5l5IKJBvK6sPTKa2WZ1js3d947pvWXbPJ1OmWfEuxLgeHcEbjUUA5i9V5ZkpCw" crossorigin="anonymous"></script>
            <script>
                document.addEventListener("htmx:configRequest", (evt) => {
                    let httpVerb = evt.detail.verb.toUpperCase();
                    if (httpVerb === 'GET') return;
                    
                    let antiForgery = htmx.config.antiForgery;
                    if (antiForgery) {
                        // already specified on form, short circuit
                        if (evt.detail.parameters[antiForgery.formFieldName])
                            return;
                        
                        if (antiForgery.headerName) {
                            evt.detail.headers[antiForgery.headerName]
                                = antiForgery.requestToken;
                        } else {
                            evt.detail.parameters[antiForgery.formFieldName]
                                = antiForgery.requestToken;
                        }
                    }
                });
            </script>
            </body>
        </html>
    """;
    return Results.Content(html, "text/html");
});

var htmx = app.MapGroup("/htmx").AddEndpointFilter(async (context, next) =>
{
    if (context.HttpContext.Request.IsHtmx() is false)
        return Results.Content("");

    if (context.HttpContext.Request.Method == "GET")
        return await next(context);

    await context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>()!.ValidateRequestAsync(context.HttpContext);
    return await next(context);
});

htmx.MapGet("/", (HttpRequest request) =>
{
    request.IsHtmx(out var headers);

    return Results.Content($"GET => {DateTime.UtcNow} hello {headers.Prompt}");
});

htmx.MapPost("/", (HttpRequest request) =>
{
    request.IsHtmx(out var headers);

    return Results.Content($"POST => {DateTime.UtcNow} hello {headers.Prompt}");
});

htmx.MapDelete("/", (HttpRequest request) =>
{
    request.IsHtmx(out var headers);

    return Results.Content($"DELETE => {DateTime.UtcNow} hello {headers.Prompt}");
});

htmx.MapPut("/", (HttpRequest request) =>
{
    request.IsHtmx(out var headers);

    return Results.Content($"PUT => {DateTime.UtcNow} hello {headers.Prompt}");
});

htmx.MapPatch("/", (HttpRequest request) =>
{
    request.IsHtmx(out var headers);

    return Results.Content($"PATCH => {DateTime.UtcNow} hello {headers.Prompt}");
});

app.Run();