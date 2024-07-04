using Htmx;
var app = WebApplication.Create();
app.MapGet("/", () =>
{
    var html = """
        <!DOCTYPE html>
        <html>
            <head>
                <title>hx-boost</title>
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
            </head>
            <body style="background-color:purple;">
                <div class="container">
                    <h1>hx-boost</h1>
                    <ul hx-boost="true">
                        <li><a href="/htmx/one">One</a></li>
                        <li><a href="/htmx/two">two</a></li>
                    </ul>
                </div>
                <script src="https://unpkg.com/htmx.org@2.0.0" integrity="sha384-wS5l5IKJBvK6sPTKa2WZ1js3d947pvWXbPJ1OmWfEuxLgeHcEbjUUA5i9V5ZkpCw" crossorigin="anonymous"></script>
            </body>
        </html>
    """;
    return Results.Content(html, "text/html");
});

app.MapGet("/htmx/{key}", (HttpRequest request, string key) =>
{
    if (request.IsHtmx() is false)
        return Results.Content("");

    return key switch 
    {
        "one" => Results.Content(
            $"""
                <div class="container" style="background-color:green;color:white;">
                    <h1>ONE</h1>

                    <ul hx-boost="true">
                        <li><a href="/htmx/one">One</a></li>
                        <li><a href="/htmx/two">two</a></li>
                    </ul>
                    <script src="https://unpkg.com/htmx.org@2.0.0" integrity="sha384-wS5l5IKJBvK6sPTKa2WZ1js3d947pvWXbPJ1OmWfEuxLgeHcEbjUUA5i9V5ZkpCw" crossorigin="anonymous"></script>
                </div>  
            """),
            "two" => Results.Content(
            $"""
                <div class="container" style="background-color:yellow;color:red;">
                    <h1>TWO</h1>

                    <ul hx-boost="true">
                        <li><a href="/htmx/one">One</a></li>
                        <li><a href="/htmx/two">two</a></li>
                    </ul>

                    <script src="https://unpkg.com/htmx.org@2.0.0" integrity="sha384-wS5l5IKJBvK6sPTKa2WZ1js3d947pvWXbPJ1OmWfEuxLgeHcEbjUUA5i9V5ZkpCw" crossorigin="anonymous"></script>
                </div>  
            """),
        _ => Results.Content("")
    };
});

app.Run();


