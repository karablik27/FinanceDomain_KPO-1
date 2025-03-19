using System.Collections.Generic;
using FinanceDataImporterExporter.Export;
using FinanceLibrary;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace financial_accounting.Export
{
    /// <summary>
    /// Экспортёр данных в формат YAML с использованием паттерна Посетитель.
    /// </summary>
    public class YamlExportVisitor : IExportVisitor
    {
        private readonly List<BankAccount> _bankAccounts = new List<BankAccount>();
        private readonly List<Category> _categories = new List<Category>();
        private readonly List<Operation> _operations = new List<Operation>();

        public void Visit(BankAccount bankAccount)
        {
            _bankAccounts.Add(bankAccount);
        }

        public void Visit(Category category)
        {
            _categories.Add(category);
        }

        public void Visit(Operation operation)
        {
            _operations.Add(operation);
        }

        /// <summary>
        /// Возвращает данные, экспортированные в формат YAML.
        /// </summary>
        public string GetResult()
        {
            var result = new
            {
                BankAccounts = _bankAccounts,
                Categories = _categories,
                Operations = _operations
            };

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return serializer.Serialize(result);
        }
    }
}
