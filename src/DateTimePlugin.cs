using Microsoft.SemanticKernel;

class DateTimePlugin
{
    [KernelFunction]
    public string GetCurrentDate() => DateTime.Now.Date.ToString("yyyy/MM/dd");
}