using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace FinanceDataImporterExporter.Import
{
    /// <summary>
    /// Импортер данных из YAML-файла.
    /// </summary>
    public class YamlDataImporter : DataImporter
    {
        protected override object ParseData(string content)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                // Здесь результатом может быть объект типа dynamic или Dictionary<string, object>
                var yamlObject = deserializer.Deserialize<object>(content);
                return yamlObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка парсинга YAML: {ex.Message}");
                throw;
            }
        }

        protected override void ProcessData(object data)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yaml = serializer.Serialize(data);
            Console.WriteLine("Импортированные данные (YAML):");
            Console.WriteLine(yaml);
        }
    }
}

