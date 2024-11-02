#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;

namespace semantic_kernel_sample;

class SemanticKernelService
{
    public static readonly string OpenaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        ?? throw new InvalidOperationException("Environment variable OPENAI_API_KEY not found.");
    public static readonly string ModelId = "gpt-4o-mini";  // or "gpt-3.5-turbo"
    public static readonly string EmbeddingModelId = "text-embedding-3-small";
    public Kernel Kernel { get; private set; }
    public IChatCompletionService ChatCompletionService { get; private set; }
    public ChatHistory ChatHistory { get; private set; } = new ChatHistory();
    private OpenAIPromptExecutionSettings _autoInvokeSettings =
        new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

    public SemanticKernelService()
    {
        Kernel = Kernel.CreateBuilder().AddOpenAIChatCompletion(ModelId, OpenaiApiKey).Build();
        ChatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
    }

    public void AddPlugin<T>()
    {
        Kernel.Plugins.AddFromType<T>();
    }

    public async Task<ChatMessageContent> Ask(string prompt)
    {
        ChatHistory.AddUserMessage(prompt);
        return await ChatCompletionService.GetChatMessageContentAsync(ChatHistory, _autoInvokeSettings, Kernel);
    }

    public async Task<ChatMessageContent> AskWithoutHistory(string prompt)
    {
        return await ChatCompletionService.GetChatMessageContentAsync(prompt, _autoInvokeSettings, Kernel);
    }

    public async Task<ChatMessageContent> AskWithEmbedding(string prompt, SemanticTextMemory memory)
    {
        ChatHistory.Clear();
        ChatHistory.AddSystemMessage("Answer based only on the information below: ");
        var answers = memory.SearchAsync("ask", prompt, 3, 0.2d, true);
        await foreach (var answer in answers)
        {
            Console.WriteLine($"Using: {answer.Metadata.Text} ({answer.Relevance})");
            ChatHistory.AddSystemMessage(answer.Metadata.Text);
        }
        return await Ask(prompt);
    }

    public async Task<SemanticTextMemory> ExecuteEmbedding(Dictionary<string, string> dic)
    {
        // Using in-memory store to skip prpearing DB.
        // To use DB, use connector (e.g. Microsoft.SemanticKernel.Connectors.Chroma)
        var store = new VolatileMemoryStore();
        var embeddingGenerator = new OpenAITextEmbeddingGenerationService(EmbeddingModelId, OpenaiApiKey);
        var memory = new SemanticTextMemory(store, embeddingGenerator);
        foreach (var item in dic)
        {
            await memory.SaveInformationAsync("ask", id: item.Key, text: item.Value);
        }
        return memory;
    }
}