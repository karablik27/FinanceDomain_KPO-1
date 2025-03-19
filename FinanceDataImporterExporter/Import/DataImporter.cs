using System;
using System.IO;
namespace FinanceDataImporterExporter.Import
{
    /// <summary>
    /// Абстрактный класс для импорта данных из файла.
    /// Использует паттерн "Шаблонный метод".
    /// </summary>
    public abstract class DataImporter
    {
        /// <summary>
        /// Метод шаблона для импорта данных.
        /// Читает содержимое файла, затем вызывает методы для парсинга и обработки данных.
        /// </summary>
        /// <param name="filePath">Путь к файлу импорта.</param>
        public void Import(string filePath)
        {
            // Чтение содержимого файла
            string content = File.ReadAllText(filePath);

            // Парсинг данных (реализуется в подклассе)
            var data = ParseData(content);

            // Обработка полученных данных (опционально можно переопределить)
            ProcessData(data);
        }

        /// <summary>
        /// Абстрактный метод для парсинга содержимого файла.
        /// Каждая конкретная реализация должна реализовать свою логику парсинга.
        /// </summary>
        /// <param name="content">Содержимое файла.</param>
        /// <returns>Результат парсинга, например, массив или коллекцию объектов.</returns>
        protected abstract object ParseData(string content);

        /// <summary>
        /// Метод для обработки импортированных данных.
        /// Может быть переопределён в подклассах для специфичной обработки.
        /// </summary>
        /// <param name="data">Данные, полученные после парсинга.</param>
        protected virtual void ProcessData(object data)
        {
            // Базовая реализация: вывод информации в консоль.
            Console.WriteLine("Импортированные данные:");
            Console.WriteLine(data?.ToString());
        }
    }
}

