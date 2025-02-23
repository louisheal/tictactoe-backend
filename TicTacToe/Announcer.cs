using System.Collections.Concurrent;
using System.Text.Json;

namespace TicTacToe;

public class Announcer
{
    private readonly ConcurrentDictionary<int, HttpResponse> _clients = new();
    private int _nextId;
    
    public void AddClient(HttpResponse response)
    {
        _clients.TryAdd(_nextId++, response);
    }

    public async void AnnounceState(GameDto game)
    {
        var message = JsonSerializer.Serialize(game);
        await Announce(message);
    }

    private async Task Announce(string message)
    {
        var disconnectedClients = new ConcurrentBag<int>();
        
        foreach (var item in _clients)
        {
            try
            {
                if (!item.Value.HttpContext.RequestAborted.IsCancellationRequested)
                {
                    await item.Value.WriteAsync($"data: {message}\n\n");
                    await item.Value.Body.FlushAsync();
                }
                else
                {
                    disconnectedClients.Add(item.Key);
                }
            }
            catch
            {
                disconnectedClients.Add(item.Key);
            }
        }

        foreach (var id in disconnectedClients)
        {
            _clients.Remove(id, out _);
        }
    }
}