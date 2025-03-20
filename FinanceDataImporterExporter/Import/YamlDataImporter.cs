using System;
using System.Collections.Generic;
using System.Linq;
using FinanceLibrary;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FinanceDataImporterExporter.Import
{
    public class YamlDataImporter : DataImporter<FinanceDataExport>
    {
        protected override FinanceDataExport ParseData(string content)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            // Десериализуем YAML как объект
            var rawObject = deserializer.Deserialize<object>(content);

            IDictionary<string, object> rootDict = null;

            // Если возвращается IDictionary<object, object>, преобразуем ключи в строки
            if (rawObject is IDictionary<object, object> dict)
            {
                rootDict = dict.ToDictionary(k => k.Key.ToString(), v => v.Value);
            }
            // Если уже IDictionary<string, object>, то используем напрямую
            else if (rawObject is IDictionary<string, object> stringDict)
            {
                rootDict = stringDict;
            }
            else
            {
                throw new FormatException("Некорректный формат YAML (ожидался словарь).");
            }

            var exportData = new FinanceDataExport
            {
                BankAccounts = new List<BankAccount>(),
                Categories = new List<Category>(),
                Operations = new List<Operation>()
            };

            // Обработка счетов
            if (rootDict.TryGetValue("bankAccounts", out object bankAccountsObj) && bankAccountsObj is IList<object> bankAccountsList)
            {
                foreach (var item in bankAccountsList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["id"].ToString());
                        var name = itemDict["name"].ToString();
                        var balance = decimal.Parse(itemDict["balance"].ToString());
                        exportData.BankAccounts.Add(new BankAccount(id, name, balance));
                    }
                }
            }

            // Обработка категорий
            if (rootDict.TryGetValue("categories", out object categoriesObj) && categoriesObj is IList<object> categoriesList)
            {
                foreach (var item in categoriesList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["id"].ToString());
                        var type = (CategoryType)Enum.Parse(typeof(CategoryType), itemDict["type"].ToString(), true);
                        var name = itemDict["name"].ToString();
                        exportData.Categories.Add(new Category(id, type, name));
                    }
                }
            }

            // Обработка операций
            if (rootDict.TryGetValue("operations", out object operationsObj) && operationsObj is IList<object> operationsList)
            {
                foreach (var item in operationsList)
                {
                    if (item is IDictionary<object, object> itemDictObj)
                    {
                        var itemDict = itemDictObj.ToDictionary(k => k.Key.ToString(), v => v.Value);
                        var id = Guid.Parse(itemDict["id"].ToString());
                        var type = (OperationType)Enum.Parse(typeof(OperationType), itemDict["type"].ToString(), true);
                        var bankAccountId = Guid.Parse(itemDict["bankAccountId"].ToString());
                        var amount = decimal.Parse(itemDict["amount"].ToString());
                        var date = DateTime.Parse(itemDict["date"].ToString());
                        var description = itemDict["description"].ToString();
                        var categoryId = Guid.Parse(itemDict["categoryId"].ToString());
                        exportData.Operations.Add(new Operation(id, type, bankAccountId, amount, date, description, categoryId));
                    }
                }
            }

            return exportData;
        }

        protected override void ProcessData(FinanceDataExport data)
        {
            Console.WriteLine("Импорт YAML выполнен успешно.");
            Console.WriteLine($"Счетов: {data.BankAccounts.Count}, Категорий: {data.Categories.Count}, Операций: {data.Operations.Count}");
        }
    }
}
