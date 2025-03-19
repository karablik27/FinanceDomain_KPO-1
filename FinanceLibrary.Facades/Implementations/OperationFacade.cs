namespace FinanceLibrary.Facades.Implementations
{
    public class OperationFacade : IOperationFacade
    {
        private readonly DomainFactory _factory;
        private readonly Dictionary<Guid, Operation> _operations;

        public OperationFacade(DomainFactory factory)
        {
            _factory = factory;
            _operations = new Dictionary<Guid, Operation>();
        }

        public Operation Create(OperationType type, Guid bankAccountId, decimal amount, Guid categoryId, string description = null)
        {
            var operation = _factory.CreateOperation(type, bankAccountId, amount, categoryId, description);
            _operations[operation.Id] = operation;
            return operation;
        }

        public void Delete(Guid id)
        {
            if (!_operations.ContainsKey(id))
                throw new KeyNotFoundException("Операция не найдена.");

            _operations.Remove(id);
        }

        public Operation GetById(Guid id)
        {
            if (!_operations.ContainsKey(id))
                throw new KeyNotFoundException("Операция не найдена.");

            return _operations[id];
        }

        public IEnumerable<Operation> GetAll()
        {
            return _operations.Values;
        }

        public IEnumerable<Operation> GetByAccount(Guid accountId)
        {
            return _operations.Values.Where(o => o.BankAccountId == accountId);
        }

        public IEnumerable<Operation> GetByCategory(Guid categoryId)
        {
            return _operations.Values.Where(o => o.CategoryId == categoryId);
        }

        public IEnumerable<Operation> GetByDateRange(DateTime start, DateTime end)
        {
            return _operations.Values.Where(o => o.Date >= start && o.Date <= end);
        }
    }
} 