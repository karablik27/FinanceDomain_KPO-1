using System;
using System.Text.Json;

namespace FinanceDataImporterExporter.Import
{
    /// <summary>
    /// Импортер данных из JSON-файла.
    /// </summary>
    public class JsonDataImporter : DataImporter
    {
        protected override object ParseData(string content)
        {
            try
            {
                // Парсинг содержимого в JsonDocument
                JsonDocument doc = JsonDocument.Parse(content);
                return doc;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга JSON: {ex.Message}");
                throw;
            }
        }

        protected override void ProcessData(object data)
        {
            if (data is JsonDocument doc)
            {
                // Форматированный вывод с отступами
                var options = new JsonSerializerOptions { WriteIndented = true };
                string formattedJson = JsonSerializer.Serialize(doc.RootElement, options);
                Console.WriteLine("Импортированные данные (JSON):");
                Console.WriteLine(formattedJson);
            }
            else
            {
                base.ProcessData(data);
            }
        }
    }
}

