using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Register SignalR services
builder.Services.AddSignalR();

var app = builder.Build();

// Basic test endpoint
app.MapGet("/", () => "SignalR Hello World Server is running.");

// Map the SignalR hub
app.MapHub<ChatHub>("/chatHub");

app.Run();

// Define the Hub class
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected: {Context.ConnectionId}");
        await Clients.Caller.SendAsync("ReceiveMessage", "Server", "Hello from the SignalR server!");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"{user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}
