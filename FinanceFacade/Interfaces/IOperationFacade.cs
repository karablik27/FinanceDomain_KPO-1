using System;
using System.Collections.Generic;
using FinanceLibrary;

namespace FinanceFacade.Interfaces
{
    public interface IOperationFacade
    {
        Operation Create(OperationType type, Guid bankAccountId, decimal amount, Category category, string description = null);
        void Delete(Guid id);
        Operation GetById(Guid id);
        IEnumerable<Operation> GetAll();
        IEnumerable<Operation> GetByAccount(Guid accountId);
        IEnumerable<Operation> GetByCategory(Guid categoryId);
        IEnumerable<Operation> GetByDateRange(DateTime start, DateTime end);
    }
}
