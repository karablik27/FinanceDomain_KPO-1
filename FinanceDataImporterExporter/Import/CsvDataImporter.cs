using System;
namespace FinanceDataImporterExporter.Import
{
    /// <summary>
    /// Импортер данных из CSV-файла.
    /// </summary>
    public class CsvDataImporter : DataImporter
    {
        protected override object ParseData(string content)
        {
            // Разбиваем содержимое на строки
            string[] lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            // Преобразуем строки в список массивов (ячейки)
            List<string[]> csvData = new List<string[]>();
            foreach (var line in lines)
            {
                // Игнорируем строки, начинающиеся с '#' (комментарии)
                if (line.TrimStart().StartsWith("#"))
                    continue;
                string[] cells = line.Split(',');
                csvData.Add(cells);
            }
            return csvData;
        }

        protected override void ProcessData(object data)
        {
            if (data is List<string[]> csvData)
            {
                Console.WriteLine("Импортированные данные (CSV):");
                foreach (var row in csvData)
                {
                    Console.WriteLine(string.Join(" | ", row));
                }
            }
            else
            {
                base.ProcessData(data);
            }
        }
    }
}

