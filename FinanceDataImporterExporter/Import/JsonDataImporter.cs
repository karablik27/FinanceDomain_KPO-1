using System.Text.Json;
using FinanceLibrary;

namespace FinanceDataImporterExporter.Import
{
    public class JsonDataImporter : DataImporter<FinanceDataExport>
    {
        protected override FinanceDataExport ParseData(string content)
        {
            return JsonSerializer.Deserialize<FinanceDataExport>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

        protected override void ProcessData(FinanceDataExport data)
        {
            Console.WriteLine("JSON импорт выполнен успешно.");
            Console.WriteLine($"Счетов: {data.BankAccounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }
}
