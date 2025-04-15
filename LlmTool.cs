using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MC;


[McpServerToolType]
public sealed class LlmTool
{
    [McpServerTool(Name = "LLM"), Description("Samples from an LLM using MCP's sampling feature")]
    public static async Task<string> SampleLLM(
        IMcpServer thisServer,
        [Description("The prompt to send to the LLM")] string prompt,
        [Description("Maximum number of tokens to generate")] int maxTokens,
        CancellationToken cancellationToken)
    {
        var chatClient = thisServer.AsSamplingChatClient() ?? throw new InvalidOperationException("This server does not support sampling.");

        Console.WriteLine($"[LLM] Request received. Prompt: '{prompt}', MaxTokens: {maxTokens}");
        Console.WriteLine($"[LLM] Sampling parameters created: {System.Text.Json.JsonSerializer.Serialize(CreateRequestSamplingParams(prompt ?? string.Empty, "LLM", maxTokens))}");
        
        var samplingParams = CreateRequestSamplingParams(prompt ?? string.Empty, "LLM", maxTokens);
        var sampleResult = await thisServer.RequestSamplingAsync(samplingParams, cancellationToken);

        return $"LLM sampling result: {sampleResult.Content.Text}";
    }



/*   

        try
        {
            Console.WriteLine($"[LLM] Request received. Prompt: '{prompt}', MaxTokens: {maxTokens}");
            var chatClient = thisServer.AsSamplingChatClient() ?? throw new InvalidOperationException("This server does not support sampling.");
            var samplingParams = CreateRequestSamplingParams(prompt ?? string.Empty, "LLM", maxTokens);
            Console.WriteLine($"[LLM] Sampling parameters created: {System.Text.Json.JsonSerializer.Serialize(samplingParams)}");
            var sampleResult = await thisServer.RequestSamplingAsync(samplingParams, cancellationToken);
            Console.WriteLine($"[LLM] Sampling result: {sampleResult.Content.Text}");
            return $"LLM sampling result: {sampleResult.Content.Text}";
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[LLM] Error: {ex.Message}\n{ex.StackTrace}");
            return $"LLM error: {ex.Message}";
        }


     var chatClientParams = new ChatClientRequestParams()
        {
            Messages = [new SamplingMessage()
                {
                    Role = Role.User,
                    Content = new Content()
                    {
                        Type = "text",
                        Text = $"Resource LLM context: {prompt}"
                    }
                }],
            SystemPrompt = "You are a helpful test server.",
            MaxTokens = maxTokens,
            Temperature = 0.7f,
            IncludeContext = ContextInclusion.ThisServer
        };*/
    private static CreateMessageRequestParams CreateRequestSamplingParams(string context, string uri, int maxTokens = 100)
    {
        return new CreateMessageRequestParams()
        {
            Messages = [new SamplingMessage()
                {
                    Role = Role.User,
                    Content = new Content()
                    {
                        Type = "text",
                        Text = $"Resource {uri} context: {context}"
                    }
                }],
            SystemPrompt = "You are a helpful test server.",
            MaxTokens = maxTokens,
            Temperature = 0.7f,
            IncludeContext = ContextInclusion.ThisServer
        };
    }
}