using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerPromptType]
public class TellerPrompts
{
    [McpServerPrompt(Name = "currency"), Description("Creates a prompt to summarize the provided currency.")]
    public static string Summarize([Description("The currency to summarize")] string content) =>
        new string( $"Currency {content}");
}