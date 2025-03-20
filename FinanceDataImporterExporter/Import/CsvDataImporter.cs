using System;
using System.Collections.Generic;
using System.Linq;
using FinanceLibrary;

namespace FinanceDataImporterExporter.Import
{
    public class CsvDataImporter : DataImporter<FinanceDataExport>
    {
        protected override FinanceDataExport ParseData(string content)
        {
            var exportData = new FinanceDataExport
            {
                BankAccounts = new List<BankAccount>(),
                Categories = new List<Category>(),
                Operations = new List<Operation>()
            };

            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var cells = line.Split(',');
                if (cells.Length == 0)
                    continue;
                string recordType = cells[0].Trim();
                if (recordType == "BankAccount" && cells.Length >= 4)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    string name = cells[2].Trim();
                    decimal balance = decimal.Parse(cells[3].Trim());
                    exportData.BankAccounts.Add(new BankAccount(id, name, balance));
                }
                else if (recordType == "Category" && cells.Length >= 4)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    // Используем тип CategoryType (Income/Expense)
                    CategoryType catType = (CategoryType)Enum.Parse(typeof(CategoryType), cells[2].Trim(), true);
                    string name = cells[3].Trim();
                    exportData.Categories.Add(new Category(id, catType, name));
                }
                else if (recordType == "Operation" && cells.Length >= 8)
                {
                    Guid id = Guid.Parse(cells[1].Trim());
                    OperationType opType = (OperationType)Enum.Parse(typeof(OperationType), cells[2].Trim(), true);
                    Guid bankAccountId = Guid.Parse(cells[3].Trim());
                    decimal amount = decimal.Parse(cells[4].Trim());
                    DateTime date = DateTime.Parse(cells[5].Trim());
                    string description = cells[6].Trim();
                    Guid categoryId = Guid.Parse(cells[7].Trim());
                    exportData.Operations.Add(new Operation(id, opType, bankAccountId, amount, date, description, categoryId));
                }
            }
            return exportData;
        }

        protected override void ProcessData(FinanceDataExport data)
        {
            Console.WriteLine("CSV импорт выполнен успешно.");
            Console.WriteLine($"Счетов: {data.BankAccounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }
}
