using System;
using FinanceFacade.Interfaces;

public class DeleteOperationCommand : ICommand
{
    private readonly IOperationFacade _operationFacade;
    private readonly Guid _operationId;

    public DeleteOperationCommand(IOperationFacade operationFacade, Guid operationId)
    {
        _operationFacade = operationFacade;
        _operationId = operationId;
    }

    public void Execute()
    {
        _operationFacade.Delete(_operationId);
        Console.WriteLine($"Операция {_operationId} удалена.");
    }
}
