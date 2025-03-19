namespace FinanceLibrary.Facades.Interfaces
{
    public interface IOperationFacade
    {
        Operation Create(OperationType type, Guid bankAccountId, decimal amount, Guid categoryId, string description = null);
        void Delete(Guid id);
        Operation GetById(Guid id);
        IEnumerable<Operation> GetAll();
        IEnumerable<Operation> GetByAccount(Guid accountId);
        IEnumerable<Operation> GetByCategory(Guid categoryId);
        IEnumerable<Operation> GetByDateRange(DateTime start, DateTime end);
    }
} 