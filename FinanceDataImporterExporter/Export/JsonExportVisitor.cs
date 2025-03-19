﻿using System.Collections.Generic;
using System.Text.Json;
using FinanceDataImporterExporter.Export;
using FinanceLibrary; // Здесь находятся ваши доменные классы (BankAccount, Category, Operation)

namespace financial_accounting.Export
{
    /// <summary>
    /// Экспортёр данных в формат JSON с использованием паттерна Посетитель.
    /// </summary>
    public class JsonExportVisitor : IExportVisitor
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
        /// Возвращает данные, экспортированные в форматированный JSON.
        /// </summary>
        public string GetResult()
        {
            var result = new
            {
                BankAccounts = _bankAccounts,
                Categories = _categories,
                Operations = _operations
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(result, options);
        }
    }
}
