using static Sample;

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
}
