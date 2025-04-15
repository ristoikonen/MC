using MC;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // All logs to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

//builder.Services.AddChatClient( new OllamaChatClient(new Uri(@"http://localhost:11434"), "llama3.2:latest"));
//OllamaChatClient occ = new OllamaChatClient(new Uri(@"http://localhost:11434"), "llama3.2");
//builder.Services.AddSingleton<IChatClient>(occ.AsBuilder().UseFunctionInvocation().UseOpenTelemetry().Build());
//Console.WriteLine("added chat client");

builder.Services
    .AddHttpClient()
    .AddSingleton<MonkeyService>()
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithTools<LlmTool>()
    .WithPrompts<TellerPrompts>();

//  OllamaPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

await builder.Build().RunAsync();



