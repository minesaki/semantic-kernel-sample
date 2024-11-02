namespace semantic_kernel_sample;

class Program
{
    static async Task Main(string[] args)
    {
        DotNetEnv.Env.Load();       // Load .env file

        // Sample 1: Chat history
        await ChatHistorySample();

        // Sample 2: Function calling
        await FunctionCallingSample();

        // Sample 3: RAG
        await RagSample();
    }

    static async Task ChatHistorySample()
    {
        Console.WriteLine("[ChatHistorySample]");
        var sk = new SemanticKernelService();
        string prompt;

        Console.WriteLine($"=== Case 1: Talk without history ===");
        prompt = "What's Internet? Answer within 200 characters.";
        WritePrompt(prompt);
        WriteResponse(await sk.AskWithoutHistory(prompt));
        prompt = "日本語で";        // in Japanese
        WritePrompt(prompt);
        WriteResponse(await sk.AskWithoutHistory(prompt));    // Would respond NOT based on previous conversation

        Console.WriteLine($"=== Case 2: Talk with history ===");
        prompt = "What's Internet? Answer within 200 characters.";
        WritePrompt(prompt);
        WriteResponse(await sk.Ask(prompt));
        prompt = "日本語で";        // in Japanese
        WritePrompt(prompt);
        WriteResponse(await sk.Ask(prompt));                  // Would respond based on previous conversation
        Console.WriteLine();
    }

    static async Task FunctionCallingSample()
    {
        Console.WriteLine("[FunctionCallingSample]");
        var sk = new SemanticKernelService();
        string prompt;

        Console.WriteLine($"=== Case 1: Asking today's date ({SemanticKernelService.ModelId}) ===");
        prompt = "What's the date today?";
        WritePrompt(prompt);
        WriteResponse(await sk.Ask(prompt));        // Would respond wrong date (or can't answer, depending on LLM model)

        sk.AddPlugin<DateTimePlugin>();             // Register plugin (=function) to tell LLM today's date

        Console.WriteLine($"=== Case 2: Asking today's date ({SemanticKernelService.ModelId} with function calling) ===");
        WritePrompt(prompt);
        WriteResponse(await sk.Ask(prompt));        // Thanks to the plugin, it would respond correct date
        Console.WriteLine();
    }

    static async Task RagSample()
    {
        Console.WriteLine("[EmbeddingSample]");
        var sk = new SemanticKernelService();
        string prompt;

        // Generate embedding (vector) data and store in memory
        // (This dummy person info was generated with ChatGPT)
        var memory = await sk.ExecuteEmbedding(new Dictionary<string, string>
        {
            {"name", "My name is Satoshi Nakamura." },
            {"age", "I'm 32 years old." },
            {"job", "I'm a graphic designer." },
            {"birthplace", "I was born in Kyoto prefecture."},
            {"academic background", "I graduated from the Faculty of Fine Arts at Kyoto University of Arts."},
            {"hobby", "My hobbies are outdoor activities (hiking, camping) and photography."},
            {"skill", "My specialty is creating digital illustrations."},
            {"personality", "My personality is easy-going, sociable, and curious."},
            {"favorite food", "My favorite food is Japanese food in general, especially sushi."},
            {"dream", "My dream is to have my own design studio"},
        });

        Console.WriteLine($"=== Case: Asking questions based on RAG ===");
        prompt = "What's your name?";
        WritePrompt(prompt);
        WriteResponse(await sk.AskWithEmbedding(prompt, memory));
        prompt = "Please introduce yourself.";
        WritePrompt(prompt);
        WriteResponse(await sk.AskWithEmbedding(prompt, memory));
    }


    static void WritePrompt(object data)
    {
        Console.ResetColor();
        Console.WriteLine($"> {data}");
    }

    static void WriteResponse(object data)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(data);
        Console.ResetColor();
    }

}
