using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Create from delegate
McpServerPrompt prompt = McpServerPrompt.Create(
    ([Description("Echoes the message back to the client.")] string message, [Description("An integer value to include in the prompt.")] int value) => new ChatMessage(ChatRole.User, $"The prompt is: {message} {value}"),
    new McpServerPromptCreateOptions
    {
        Name = "CustomPrompt",
        Description = "A custom prompt example"
    });

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithHttpTransport(options =>
    {
        // Configure HTTP transport options (optional)
        options.IdleTimeout = TimeSpan.FromHours(1);
    })
    .WithPrompts([prompt])
    .WithToolsFromAssembly();

var app = builder.Build();
app.MapMcp("/mcp");
await app.RunAsync();