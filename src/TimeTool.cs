using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

[McpServerToolType]
public static class EchoTool
{

    private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("EchoTool");
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message)
    {
        // log message
        logger.LogInformation("EchoTool.Echo called with message: {Message}", message);
        return $"Hello from test#: {message}";
    }


    [McpServerTool, Description("Echoes in reverse the message sent by the client.")]
    public static string ReverseEcho(string message)
    {
        // log message
        logger.LogInformation("EchoTool.ReverseEcho called with message: {Message}", message);
        return new([.. message.Reverse()]);
    }
}
