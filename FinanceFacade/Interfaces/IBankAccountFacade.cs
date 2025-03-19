using System;
using System.Collections.Generic;
using FinanceLibrary;

namespace FinanceFacade.Interfaces
{
    public interface IBankAccountFacade
    {
        BankAccount Create(string name, decimal initialBalance = 0);
        void Update(Guid id, string newName);
        void Delete(Guid id);
        BankAccount GetById(Guid id);
        IEnumerable<BankAccount> GetAll();
        void RecalculateBalance(Guid accountId, IEnumerable<Operation> operations);
    }
}
