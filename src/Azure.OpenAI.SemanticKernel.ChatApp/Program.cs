using Azure.AI.OpenAI;
using Azure.OpenAI.SemanticKernel.ChatApp;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;

internal class Program {
    private static async Task Main(string[] args) {
        LMStudio lMStudio = new LMStudio();
        await lMStudio.RunLMStudioChatbot();
    }
}