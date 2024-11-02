# Semantic Kernel Sample

Sample AI console application implemented with C#, Semantic Kernel, and OpenAI API.  
This shows how to realize chat with history, function calling, and RAG.  

## Prerequisites

- .NET 8
- OpenAI API account (< $0.01 at one execution)

## How to run

### Create .env file

Place .env file (contents are as follows) in the project root.
```
OPENAI_API_KEY=<Your OpenAI API Key>
```

### Run

``` sh
dotnet run
```

## Memo

### Commands executed to create this project
``` sh
git init
dotnet new console --use-program-main
dotnet new gitignore
dotnet add package DotNetEnv
dotnet add package Microsoft.SemanticKernel
dotnet add package Microsoft.SemanticKernel.Plugins.Memory --prerelease
```
