using Microsoft.AspNetCore.Mvc;
using TicTacToe;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5000");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

var gameManager = new GameManager();

app.MapGet("/games", gameManager.GetGames);
app.MapPost("/games", gameManager.NewGame);
app.MapGet("games/{id:int}", (int id) => gameManager.GetGameState(id));
app.MapPost("/games/{id:int}", (int id, [FromBody] Move move) => gameManager.Move(id, move));
app.MapGet("/games/{id:int}/listen", async (int id, HttpContext context) =>
{
    context.Response.Headers.Append("Content-Type", "text/event-stream");
    context.Response.Headers.Append("Cache-Control", "no-cache");
    context.Response.Headers.Append("Connection", "keep-alive");

    var announcer = gameManager.GetAnnouncer(id);
    announcer.AddClient(context.Response);

    try
    {
        while (!context.RequestAborted.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }
    }
    catch
    {
        // Client disconnected, no action needed
    }
});

app.Run();
