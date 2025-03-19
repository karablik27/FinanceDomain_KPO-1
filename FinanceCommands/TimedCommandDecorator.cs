using System;
using System.Diagnostics;

public class TimedCommandDecorator : ICommand
{
    private readonly ICommand _innerCommand;

    public TimedCommandDecorator(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        _innerCommand.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения команды: {stopwatch.ElapsedMilliseconds} мс");
    }
}
