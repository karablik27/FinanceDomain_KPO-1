using System.IO;

namespace FinanceDataImporterExporter.Import
{
    /// <summary>
    /// Абстрактный класс для импорта данных из файла с использованием паттерна "Шаблонный метод".
    /// </summary>
    /// <typeparam name="T">Тип данных, который будет возвращён после импорта (например, FinanceDataExport).</typeparam>
    public abstract class DataImporter<T>
    {
        /// <summary>
        /// Общий алгоритм импорта: читает содержимое файла, вызывает метод парсинга, затем обрабатывает данные.
        /// </summary>
        /// <param name="filePath">Путь к файлу импорта.</param>
        /// <returns>Импортированные данные типа T.</returns>
        public T Import(string filePath)
        {
            // Чтение содержимого файла
            string content = File.ReadAllText(filePath);

            // Парсинг данных – делегируется конкретному классу-наследнику
            T data = ParseData(content);

            // Обработка данных (можно переопределить, если требуется)
            ProcessData(data);

            return data;
        }

        /// <summary>
        /// Абстрактный метод для парсинга содержимого файла.
        /// Каждая конкретная реализация должна реализовать свою логику парсинга.
        /// </summary>
        /// <param name="content">Содержимое файла.</param>
        /// <returns>Десериализованный объект типа T.</returns>
        protected abstract T ParseData(string content);

        /// <summary>
        /// Метод для обработки импортированных данных.
        /// По умолчанию выводит данные в консоль.
        /// </summary>
        /// <param name="data">Импортированные данные.</param>
        protected virtual void ProcessData(T data)
        {
            Console.WriteLine("Импортированные данные:");
            Console.WriteLine(data?.ToString());
        }
    }
}
