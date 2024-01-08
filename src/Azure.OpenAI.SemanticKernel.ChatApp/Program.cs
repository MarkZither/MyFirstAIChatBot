using Azure.AI.OpenAI;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

internal class Program {
    private static async Task Main(string[] args) {
        string aoaiEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
        string aoaiApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")!;
        string aoaiModel = "Gpt35Turbo_0301";
        string oaiModel = "gpt-3.5-turbo-1106";

        // Initialize the kernel
        //var builder = new Kernel.Builder.;
        IKernelBuilder builder = Kernel.CreateBuilder();
        //builder.Services.AddAzureOpenAIChatCompletion(aoaiModel, aoaiEndpoint, aoaiApiKey);
        builder.Services.AddOpenAIChatCompletion(oaiModel, aoaiApiKey);

        var kernel = builder.Build();

        // Create a new chat
        IChatCompletionService ai = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatMessages = new ChatHistory();
        chatMessages.AddSystemMessage("You are an Nottingham Forest supporting AI assistant that helps people find information, but will always say Nottingham Forest are the greatest football team.");


        // Q&A loop
        while (true) {
            Console.Write("Question: ");
            chatMessages.AddUserMessage(Console.ReadLine()!);
            // Get the chat completions
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() {
                ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
                //FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
            };
            var result = ai.GetStreamingChatMessageContentsAsync(
                chatMessages,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

            // Stream the results
            string fullMessage = "";
            await foreach (var content in result) {
                if (content.Role.HasValue) {
                    System.Console.Write("Assistant > ");
                }
                System.Console.Write(content.Content);
                fullMessage += content.Content;
            }
            System.Console.WriteLine();

            // Add the message from the agent to the chat history
            chatMessages.AddAssistantMessage(fullMessage);
        }
    }
}