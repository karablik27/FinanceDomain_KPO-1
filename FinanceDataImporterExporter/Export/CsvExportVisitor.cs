using System;
using System.Text;
using FinanceDataImporterExporter.Export;
using FinanceLibrary;

namespace financial_accounting.Export
{
    /// <summary>
    /// Конкретный посетитель, который формирует CSV-строку.
    /// </summary>
    public class CsvExportVisitor : IExportVisitor
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public void Visit(BankAccount bankAccount)
        {
            _sb.AppendLine($"BankAccount,{bankAccount.Id},{bankAccount.Name},{bankAccount.Balance}");
        }

        public void Visit(Category category)
        {
            _sb.AppendLine($"Category,{category.Id},{category.Type},{category.Name}");
        }

        public void Visit(Operation operation)
        {
            _sb.AppendLine($"Operation,{operation.Id},{operation.Type},{operation.BankAccountId},{operation.Amount},{operation.Date:O},{operation.Description},{operation.CategoryId}");
        }

        public string GetResult()
        {
            return _sb.ToString();
        }
    }
}
