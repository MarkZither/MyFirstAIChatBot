using Microsoft.SemanticKernel;

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

        // Q&A loop
        while (true) {
            Console.Write("Question: ");
            Console.WriteLine((await kernel.InvokePromptAsync(Console.ReadLine()!)).GetValue<string>());
            Console.WriteLine();
        }
    }
}