static class Lib
{
    public static void WritePrompt(object data)
    {
        Console.ResetColor();
        Console.WriteLine($"> {data}");
    }

    public static void WriteResponse(object data)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(data);
        Console.ResetColor();
    }

    public static void WriteDebug(object data)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(data);
        Console.ResetColor();
    }

}