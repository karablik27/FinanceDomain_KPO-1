using System;
using System.Collections.Generic;
using System.Linq;
using FinanceLibrary;  // BankAccount, Category, Operation, ImportExportData

namespace FinTech
{
    public class FinanceManagerProxy : IFinanceManager
    {
        private readonly IFinanceManager _realManager;
        private IEnumerable<BankAccount> _cachedAccounts;
        private IEnumerable<Category> _cachedCategories;
        private IEnumerable<Operation> _cachedOperations;
        private DateTime _cacheTimestamp;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public FinanceManagerProxy(IFinanceManager realManager)
        {
            _realManager = realManager ?? throw new ArgumentNullException(nameof(realManager));
            RefreshCache();
        }

        private void RefreshCache()
        {
            Console.WriteLine("Обновление кэша...");
            _cachedAccounts = _realManager.GetBankAccounts();
            _cachedCategories = _realManager.GetCategories();
            _cachedOperations = _realManager.GetOperations();
            _cacheTimestamp = DateTime.Now;
        }

        private void EnsureCacheValid()
        {
            if ((DateTime.Now - _cacheTimestamp) > _cacheExpiration)
            {
                RefreshCache();
            }
        }

        // Счета
        public IEnumerable<BankAccount> GetBankAccounts()
        {
            EnsureCacheValid();
            return _cachedAccounts;
        }

        public void AddBankAccount(BankAccount account)
        {
            _realManager.AddBankAccount(account);
            RefreshCache();
        }

        public void UpdateBankAccount(BankAccount account)
        {
            _realManager.UpdateBankAccount(account);
            RefreshCache();
        }

        public void RemoveBankAccount(Guid accountId)
        {
            _realManager.RemoveBankAccount(accountId);
            RefreshCache();
        }

        // Категории
        public IEnumerable<Category> GetCategories()
        {
            EnsureCacheValid();
            return _cachedCategories;
        }

        public void AddCategory(Category category)
        {
            _realManager.AddCategory(category);
            RefreshCache();
        }

        public void UpdateCategory(Category category)
        {
            _realManager.UpdateCategory(category);
            RefreshCache();
        }

        public void RemoveCategory(Guid categoryId)
        {
            _realManager.RemoveCategory(categoryId);
            RefreshCache();
        }

        // Операции
        public IEnumerable<Operation> GetOperations()
        {
            EnsureCacheValid();
            return _cachedOperations;
        }

        public void AddOperation(Operation operation)
        {
            _realManager.AddOperation(operation);
            RefreshCache();
        }

        public void UpdateOperation(Operation operation)
        {
            _realManager.UpdateOperation(operation);
            RefreshCache();
        }

        public void RemoveOperation(Guid operationId)
        {
            _realManager.RemoveOperation(operationId);
            RefreshCache();
        }

        // Дополнительные методы – делегируем реализацию реальному менеджеру
        public Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
        {
            return _realManager.GroupOperationsByCategory(start, end);
        }

        public void ExportToCsv(string directory)
        {
            _realManager.ExportToCsv(directory);
        }

        public void ExportToJson(string filePath)
        {
            _realManager.ExportToJson(filePath);
        }

        // Добавляем методы ExportToYaml и ImportFromYaml для соответствия интерфейсу IFinanceManager

        public void ExportToYaml(string filePath)
        {
            _realManager.ExportToYaml(filePath);
        }

        public void ImportFromCsv(string directory)
        {
            _realManager.ImportFromCsv(directory);
            RefreshCache();
        }

        public void ImportFromJson(string filePath)
        {
            _realManager.ImportFromJson(filePath);
            RefreshCache();
        }

        public void ImportFromYaml(string filePath)
        {
            _realManager.ImportFromYaml(filePath);
            RefreshCache();
        }

        public void ImportFromJsonFromData(ImportExportData data)
        {
            _realManager.ImportFromJsonFromData(data);
            RefreshCache();
        }
    }
}
