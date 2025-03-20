using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FinanceLibrary;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using FinanceDataImporterExporter.Import;    // Импортёры (DataImporter<T>, CsvDataImporter, JsonDataImporter, YamlDataImporter)
using FinanceDataImporterExporter.Export;    // Экспортёры (CsvExportVisitor, JsonExportVisitor, YamlExportVisitor)

namespace FinTech
{
    public class RealFinanceManager : IFinanceManager
    {
        // Симуляция базы данных через словари
        private readonly Dictionary<Guid, BankAccount> _accounts = new Dictionary<Guid, BankAccount>();
        private readonly Dictionary<Guid, Category> _categories = new Dictionary<Guid, Category>();
        private readonly Dictionary<Guid, Operation> _operations = new Dictionary<Guid, Operation>();

        // Методы для работы со счетами
        public IEnumerable<BankAccount> GetBankAccounts() => _accounts.Values.ToList();

        public void AddBankAccount(BankAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            _accounts[account.Id] = account;
        }

        public void UpdateBankAccount(BankAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            if (!_accounts.ContainsKey(account.Id))
                throw new KeyNotFoundException("Счет не найден.");
            _accounts[account.Id] = account;
        }

        public void RemoveBankAccount(Guid accountId)
        {
            if (!_accounts.ContainsKey(accountId))
                throw new KeyNotFoundException("Счет не найден.");
            _accounts.Remove(accountId);
        }

        // Методы для работы с категориями
        public IEnumerable<Category> GetCategories() => _categories.Values.ToList();

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            _categories[category.Id] = category;
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (!_categories.ContainsKey(category.Id))
                throw new KeyNotFoundException("Категория не найдена.");
            _categories[category.Id] = category;
        }

        public void RemoveCategory(Guid categoryId)
        {
            if (!_categories.ContainsKey(categoryId))
                throw new KeyNotFoundException("Категория не найдена.");
            _categories.Remove(categoryId);
        }

        // Методы для работы с операциями
        public IEnumerable<Operation> GetOperations() => _operations.Values.ToList();

        public void AddOperation(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            _operations[operation.Id] = operation;
        }

        public void UpdateOperation(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (!_operations.ContainsKey(operation.Id))
                throw new KeyNotFoundException("Операция не найдена.");
            _operations[operation.Id] = operation;
        }

        public void RemoveOperation(Guid operationId)
        {
            if (!_operations.ContainsKey(operationId))
                throw new KeyNotFoundException("Операция не найдена.");
            _operations.Remove(operationId);
        }

        // Дополнительные методы

        public Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
        {
            return _operations.Values
                .Where(op => op.Date >= start && op.Date <= end)
                .GroupBy(op => op.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(op => op.Amount));
        }

        // --- Методы экспорта с использованием паттерна Посетитель ---

        public void ExportToCsv(string directory)
        {
            // Используем CsvExportVisitor для обхода объектов
            var visitor = new CsvExportVisitor();
            foreach (var acc in _accounts.Values)
                acc.Accept(visitor);
            foreach (var cat in _categories.Values)
                cat.Accept(visitor);
            foreach (var op in _operations.Values)
                op.Accept(visitor);
            string result = visitor.GetResult();

            string filePath = Path.Combine(directory, "export.csv");
            File.WriteAllText(filePath, result);
        }

        public void ExportToJson(string filePath)
        {
            // Используем JsonExportVisitor для обхода объектов
            var visitor = new JsonExportVisitor();
            foreach (var acc in _accounts.Values)
                acc.Accept(visitor);
            foreach (var cat in _categories.Values)
                cat.Accept(visitor);
            foreach (var op in _operations.Values)
                op.Accept(visitor);
            string result = visitor.GetResult();

            File.WriteAllText(filePath, result);
        }

        public void ExportToYaml(string filePath)
        {
            // Используем YamlExportVisitor для обхода объектов
            var visitor = new YamlExportVisitor();
            foreach (var acc in _accounts.Values)
                acc.Accept(visitor);
            foreach (var cat in _categories.Values)
                cat.Accept(visitor);
            foreach (var op in _operations.Values)
                op.Accept(visitor);
            string result = visitor.GetResult();

            File.WriteAllText(filePath, result);
        }

        // --- Методы импорта с использованием шаблонного метода ---

        public void ImportFromCsv(string directory)
        {
            // Здесь используем шаблонный метод через CsvDataImporter
            string filePath = Path.Combine(directory, "export.csv");
            var importer = new CsvDataImporter();
            FinanceDataExport data = importer.Import(filePath);
            UpdateCollections(data);
        }

        public void ImportFromJson(string filePath)
        {
            var importer = new JsonDataImporter();
            FinanceDataExport data = importer.Import(filePath);
            UpdateCollections(data);
        }

        public void ImportFromYaml(string filePath)
        {
            var importer = new YamlDataImporter();
            FinanceDataExport data = importer.Import(filePath);
            UpdateCollections(data);
        }

        public void ImportFromJsonFromData(ImportExportData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.RawData))
                throw new ArgumentException("Данные импорта пусты.", nameof(data));
            var importer = new JsonDataImporter();
            FinanceDataExport exportData = importer.Import(data.RawData); // Если DataImporter<T> перегружен для строки
            UpdateCollections(exportData);
        }

        private void UpdateCollections(FinanceDataExport exportData)
        {
            _accounts.Clear();
            _categories.Clear();
            _operations.Clear();
            foreach (var acc in exportData.BankAccounts)
                _accounts[acc.Id] = acc;
            foreach (var cat in exportData.Categories)
                _categories[cat.Id] = cat;
            foreach (var op in exportData.Operations)
                _operations[op.Id] = op;
            Console.WriteLine("Импорт выполнен успешно и коллекции обновлены.");
        }
    }
}
