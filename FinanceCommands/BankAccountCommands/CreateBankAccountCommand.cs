using System;
using FinanceFacade.Interfaces;
using FinanceLibrary;

public class CreateBankAccountCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly string _name;
    private readonly decimal _initialBalance;

    public CreateBankAccountCommand(IBankAccountFacade bankAccountFacade, string name, decimal initialBalance)
    {
        _bankAccountFacade = bankAccountFacade;
        _name = name;
        _initialBalance = initialBalance;
    }

    public void Execute()
    {
        var account = _bankAccountFacade.Create(_name, _initialBalance);
        Console.WriteLine($"Создан счет: {account}");
    }
}
