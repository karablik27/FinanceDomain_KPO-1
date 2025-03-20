using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FinanceLibrary; // Предполагается, что здесь находятся BankAccount, Category, Operation, CategoryType, OperationType, ImportExportData (если он есть)
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using FinTech; // Здесь будет класс FinanceDataExport

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

        /// <summary>
        /// Группирует операции по категории в указанном диапазоне дат.
        /// Возвращает словарь, где ключ – ID категории, а значение – сумма операций.
        /// </summary>
        public Dictionary<Guid, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
        {
            return _operations.Values
                .Where(op => op.Date >= start && op.Date <= end)
                .GroupBy(op => op.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(op => op.Amount));
        }

        /// <summary>
        /// Экспортирует данные в CSV-файл и сохраняет его в указанной директории.
        /// </summary>
        public void ExportToCsv(string directory)
        {
            string filePath = Path.Combine(directory, "export.csv");
            var lines = new List<string>();

            foreach (var acc in _accounts.Values)
                lines.Add($"BankAccount,{acc.Id},{acc.Name},{acc.Balance}");
            foreach (var cat in _categories.Values)
                lines.Add($"Category,{cat.Id},{cat.Type},{cat.Name}");
            foreach (var op in _operations.Values)
                lines.Add($"Operation,{op.Id},{op.Type},{op.BankAccountId},{op.Amount},{op.Date:O},{op.Description},{op.CategoryId}");

            File.WriteAllLines(filePath, lines);
        }

        /// <summary>
        /// Экспортирует данные в JSON-файл.
        /// </summary>
        public void ExportToJson(string filePath)
        {
            var exportData = new FinanceDataExport
            {
                BankAccounts = _accounts.Values.ToList(),
                Categories = _categories.Values.ToList(),
                Operations = _operations.Values.ToList()
            };
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(exportData, options);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Экспортирует данные в YAML-файл.
        /// </summary>
        public void ExportToYaml(string filePath)
        {
            var exportData = new FinanceDataExport
            {
                BankAccounts = _accounts.Values.ToList(),
                Categories = _categories.Values.ToList(),
                Operations = _operations.Values.ToList()
            };
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yaml = serializer.Serialize(exportData);
            File.WriteAllText(filePath, yaml);
        }

        /// <summary>
        /// Импортирует данные из CSV-файла, расположенного в указанной директории.
        /// После импорта обновляет внутренние коллекции.
        /// </summary>
        public void ImportFromCsv(string directory)
        {
            string filePath = Path.Combine(directory, "export.csv");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV файл не найден.", filePath);

            var lines = File.ReadAllLines(filePath);
            _accounts.Clear();
            _categories.Clear();
            _operations.Clear();

            foreach (var line in lines)
            {
                var cells = line.Split(',');
                if (cells.Length == 0) continue;
                string type = cells[0].Trim();
                if (type == "BankAccount" && cells.Length >= 4)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    string name = cells[2].Trim();
                    decimal balance = decimal.Parse(cells[3].Trim());
                    var account = new BankAccount(id, name, balance);
                    _accounts[id] = account;
                }
                else if (type == "Category" && cells.Length >= 4)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    CategoryType catType = (CategoryType)Enum.Parse(typeof(CategoryType), cells[2].Trim(), true);
                    string name = cells[3].Trim();
                    var category = new Category(id, catType, name);
                    _categories[id] = category;
                }
                else if (type == "Operation" && cells.Length >= 8)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    OperationType opType = (OperationType)Enum.Parse(typeof(OperationType), cells[2].Trim(), true);
                    Guid bankAccountId = Guid.Parse(cells[3].Trim());
                    decimal amount = decimal.Parse(cells[4].Trim());
                    DateTime date = DateTime.Parse(cells[5].Trim());
                    string description = cells[6].Trim();
                    Guid categoryId = Guid.Parse(cells[7].Trim());
                    var operation = new Operation(id, opType, bankAccountId, amount, date, description, categoryId);
                    _operations[id] = operation;
                }
            }
            Console.WriteLine("Импорт из CSV выполнен успешно.");
        }

        /// <summary>
        /// Импортирует данные из JSON-файла и обновляет внутренние коллекции.
        /// </summary>
        public void ImportFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("JSON файл не найден.", filePath);
            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<FinanceDataExport>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (data != null)
            {
                _accounts.Clear();
                _categories.Clear();
                _operations.Clear();
                foreach (var acc in data.BankAccounts)
                    _accounts[acc.Id] = acc;
                foreach (var cat in data.Categories)
                    _categories[cat.Id] = cat;
                foreach (var op in data.Operations)
                    _operations[op.Id] = op;
                Console.WriteLine("Импорт из JSON выполнен успешно.");
            }
            else
            {
                Console.WriteLine("Ошибка десериализации JSON.");
            }
        }

        /// <summary>
        /// Импортирует данные из YAML-файла и обновляет внутренние коллекции.
        /// </summary>
        public void ImportFromYaml(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("YAML файл не найден.", filePath);
            string yaml = File.ReadAllText(filePath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var data = deserializer.Deserialize<FinanceDataExport>(yaml);
            if (data != null)
            {
                _accounts.Clear();
                _categories.Clear();
                _operations.Clear();
                foreach (var acc in data.BankAccounts)
                    _accounts[acc.Id] = acc;
                foreach (var cat in data.Categories)
                    _categories[cat.Id] = cat;
                foreach (var op in data.Operations)
                    _operations[op.Id] = op;
                Console.WriteLine("Импорт из YAML выполнен успешно.");
            }
            else
            {
                Console.WriteLine("Ошибка десериализации YAML.");
            }
        }

        /// <summary>
        /// Импортирует данные из JSON, переданные через ImportExportData, и обновляет коллекции.
        /// </summary>
        public void ImportFromJsonFromData(ImportExportData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.RawData))
                throw new ArgumentException("Данные импорта пусты.", nameof(data));
            string jsonData = data.RawData;
            var deserialized = JsonSerializer.Deserialize<FinanceDataExport>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (deserialized != null)
            {
                _accounts.Clear();
                _categories.Clear();
                _operations.Clear();
                foreach (var acc in deserialized.BankAccounts)
                    _accounts[acc.Id] = acc;
                foreach (var cat in deserialized.Categories)
                    _categories[cat.Id] = cat;
                foreach (var op in deserialized.Operations)
                    _operations[op.Id] = op;
                Console.WriteLine("Импорт из JSON (данные) выполнен успешно.");
            }
            else
            {
                Console.WriteLine("Ошибка десериализации JSON (данные).");
            }
        }
    }
}
