using System;
using FinanceFacade.Interfaces;

public class UpdateBankAccountCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly Guid _accountId;
    private readonly string _newName;

    public UpdateBankAccountCommand(IBankAccountFacade bankAccountFacade, Guid accountId, string newName)
    {
        _bankAccountFacade = bankAccountFacade;
        _accountId = accountId;
        _newName = newName;
    }

    public void Execute()
    {
        _bankAccountFacade.Update(_accountId, _newName);
        Console.WriteLine($"Счет {_accountId} обновлен. Новое имя: {_newName}");
    }
}
