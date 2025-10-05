using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5192/chatHub") // Must match server port
    .Build();

// Event handler for messages from the server
connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

// Start the connection
await connection.StartAsync();
Console.WriteLine("Connected to SignalR Hub.");

// Send an initial test message
await connection.InvokeAsync("SendMessage", "Console Client", "Hello from WSL client!");

while (true)
{
    Console.Write("Enter message (or 'exit' to quit): ");
    var msg = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(msg)) continue;
    if (msg.ToLower() == "exit") break;

    await connection.InvokeAsync("SendMessage", "Console Client", msg);
}
