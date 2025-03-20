using System;
using System.Collections.Generic;
using FinanceFacade.Interfaces;
using FinanceLibrary;

namespace FinanceLibrary.Facades.Implementations
{
    public class BankAccountFacade : IBankAccountFacade
    {
        private readonly IDomainFactory _factory;
        private readonly Dictionary<Guid, BankAccount> _accounts;

        public BankAccountFacade(IDomainFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _accounts = new Dictionary<Guid, BankAccount>();
        }

        public BankAccount Create(string name, decimal initialBalance = 0)
        {
            var account = _factory.CreateBankAccount(name, initialBalance);
            _accounts[account.Id] = account;
            return account;
        }

        public void Update(Guid id, string newName)
        {
            if (!_accounts.ContainsKey(id))
                throw new KeyNotFoundException("Счет не найден.");

            _accounts[id].UpdateName(newName);
        }

        public void Delete(Guid id)
        {
            if (!_accounts.ContainsKey(id))
                throw new KeyNotFoundException("Счет не найден.");

            _accounts.Remove(id);
        }

        public BankAccount GetById(Guid id)
        {
            if (!_accounts.ContainsKey(id))
                throw new KeyNotFoundException("Счет не найден.");

            return _accounts[id];
        }

        public IEnumerable<BankAccount> GetAll() => _accounts.Values;

        public void RecalculateBalance(Guid accountId, IEnumerable<Operation> operations)
        {
            if (!_accounts.ContainsKey(accountId))
                throw new KeyNotFoundException("Счет не найден.");

            _accounts[accountId].RecalculateBalance(operations);
        }
    }
}
