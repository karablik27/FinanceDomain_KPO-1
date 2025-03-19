using System;
using FinanceFacade.Interfaces;
using FinanceLibrary;

public class CreateOperationCommand : ICommand
{
    private readonly IOperationFacade _operationFacade;
    private readonly OperationType _type;
    private readonly Guid _bankAccountId;
    private readonly decimal _amount;
    private readonly Category _category;
    private readonly string _description;

    public CreateOperationCommand(IOperationFacade operationFacade, OperationType type, Guid bankAccountId, decimal amount, Category category, string description = null)
    {
        _operationFacade = operationFacade;
        _type = type;
        _bankAccountId = bankAccountId;
        _amount = amount;
        _category = category;
        _description = description;
    }

    public void Execute()
    {
        var operation = _operationFacade.Create(_type, _bankAccountId, _amount, _category, _description);
        Console.WriteLine($"Создана операция: {operation}");
    }
}
