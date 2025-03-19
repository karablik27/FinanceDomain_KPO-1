using System;
using System.Collections.Generic;
using FinanceFacade.Interfaces;
using FinanceLibrary;

public class RecalculateBalanceCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly Guid _accountId;
    private readonly IEnumerable<Operation> _operations;

    public RecalculateBalanceCommand(IBankAccountFacade bankAccountFacade, Guid accountId, IEnumerable<Operation> operations)
    {
        _bankAccountFacade = bankAccountFacade;
        _accountId = accountId;
        _operations = operations;
    }

    public void Execute()
    {
        _bankAccountFacade.RecalculateBalance(_accountId, _operations);
        Console.WriteLine($"Баланс счета {_accountId} пересчитан.");
    }
}
