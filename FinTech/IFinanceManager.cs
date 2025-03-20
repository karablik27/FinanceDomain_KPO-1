using System;
using System.Collections.Generic;
using FinanceLibrary;

namespace FinTech
{
    public interface IFinanceManager
    {
        // Методы для работы со счетами
        IEnumerable<BankAccount> GetBankAccounts();
        void AddBankAccount(BankAccount account);
        void UpdateBankAccount(BankAccount account);
        void RemoveBankAccount(Guid accountId);

        // Методы для работы с категориями
        IEnumerable<Category> GetCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void RemoveCategory(Guid categoryId);

        // Методы для работы с операциями
        IEnumerable<Operation> GetOperations();
        void AddOperation(Operation operation);
        void UpdateOperation(Operation operation);
        void RemoveOperation(Guid operationId);

        // Дополнительные методы
        Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end);
        void ExportToCsv(string directory);
        void ExportToJson(string filePath);
        void ExportToYaml(string filePath);
        void ImportFromCsv(string directory);
        void ImportFromJson(string filePath);
        void ImportFromYaml(string filePath);
        void ImportFromJsonFromData(ImportExportData data);
    }
}
