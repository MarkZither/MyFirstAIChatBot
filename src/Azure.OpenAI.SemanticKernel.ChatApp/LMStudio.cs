using Azure.OpenAI.SemanticKernel.ChatApp.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Azure.OpenAI.SemanticKernel.ChatApp
{
    internal class LMStudio
    {
        public LMStudio() { }
        public async Task RunLMStudioChatbot()
        {
            // Phi-2 in LM Studio
            var phi2 = new CustomChatCompletionService();
            phi2.ModelUrl = "http://localhost:1234/v1/chat/completions";

            // semantic kernel builder
            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<IChatCompletionService>("phi2Chat", phi2);
            var kernel = builder.Build();

            // init chat
            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();

            history.AddSystemMessage("You are a useful assistant that replies using a funny style and emojis. Your name is Goku.");


            // Q&A loop
            while (true)
            {
                Console.Write("Question: ");
                history.AddUserMessage(Console.ReadLine()!);

                // print response
                var result = await chat.GetChatMessageContentsAsync(history);
                // Stream the results
                string fullMessage = "";
                foreach (var content in result)
                {
                    if (content.Role.Label.Equals("system") || content.Role.Label.Equals("user"))
                    { 
                        continue; 
                    }
                    if (content.Role.Label.Equals("assistant"))
                    {
                        System.Console.Write("Assistant > ");
                    }
                    System.Console.Write(content.Content);
                    fullMessage += content.Content;
                }
                System.Console.WriteLine();

                // Add the message from the agent to the chat history
                history.AddAssistantMessage(fullMessage);
            }
        }
    }
}
