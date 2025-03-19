using System;
using FinanceFacade.Interfaces;

public class DeleteBankAccountCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly Guid _accountId;

    public DeleteBankAccountCommand(IBankAccountFacade bankAccountFacade, Guid accountId)
    {
        _bankAccountFacade = bankAccountFacade;
        _accountId = accountId;
    }

    public void Execute()
    {
        _bankAccountFacade.Delete(_accountId);
        Console.WriteLine($"Счет {_accountId} удален.");
    }
}
