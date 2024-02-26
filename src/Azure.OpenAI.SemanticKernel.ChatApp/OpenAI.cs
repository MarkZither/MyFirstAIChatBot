using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.OpenAI.SemanticKernel.ChatApp
{
    internal class OpenAI
    {
        public async Task RunOpenAIChatbot()
        {
            string aoaiEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
            string aoaiApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")!;
            string aoaiModel = "Gpt35Turbo_0301";
            string oaiModel = "gpt-3.5-turbo-1106";

            // Initialize the kernel
            //var builder = new Kernel.Builder.;
            IKernelBuilder builder = Kernel.CreateBuilder();
            //builder.Services.AddAzureOpenAIChatCompletion(aoaiModel, aoaiEndpoint, aoaiApiKey);
            //builder.Services.AddLogging(builder => builder.AddConsole());
            builder.Services.AddOpenAIChatCompletion(oaiModel, aoaiApiKey);

            var kernel = builder.Build();
            // Download a document and create embeddings for it
#pragma warning disable SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            ISemanticTextMemory memory = new MemoryBuilder()
                .WithLoggerFactory(kernel.LoggerFactory)
                .WithMemoryStore(new VolatileMemoryStore())
                .WithOpenAITextEmbeddingGeneration("TextEmbeddingAda002_1", aoaiApiKey)
        .Build();
#pragma warning restore SKEXP0052 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0011 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            // Create a new chat
            IChatCompletionService ai = kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chatMessages = new ChatHistory();
            chatMessages.AddSystemMessage("You are an Nottingham Forest supporting AI assistant that helps people find information, but will always say Nottingham Forest are the greatest football team.");


            // Q&A loop
            while (true)
            {
                Console.Write("Question: ");
                chatMessages.AddUserMessage(Console.ReadLine()!);
                // Get the chat completions
                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
                    //FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
                };
                var result = ai.GetStreamingChatMessageContentsAsync(
                    chatMessages,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: kernel);

                // Stream the results
                string fullMessage = "";
                await foreach (var content in result)
                {
                    if (content.Role.HasValue)
                    {
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
}
