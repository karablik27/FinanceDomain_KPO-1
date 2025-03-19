using System;

namespace FinanceLibrary
{
    public interface IDomainFactory
    {
        BankAccount CreateBankAccount(string name, decimal initialBalance = 0);
        Category CreateCategory(CategoryType type, string name);
        Operation CreateOperation(OperationType type, Guid bankAccountId, decimal amount, Category category, string description = " ");
    }
}
