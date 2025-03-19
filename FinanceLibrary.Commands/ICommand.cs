using System;
using System.Diagnostics;

public interface ICommand
{
    void Execute();
}

public class PerformanceMonitorDecorator : ICommand
{
    private readonly ICommand _command;
    
    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        _command.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds}ms");
    }
} 