using System;
using System.IO;
using System.Text.Json;
using FinanceDataImporterExporter.Import;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class JsonDataImporterTests
    {
        [Fact]
        public void Import_ValidJson_ReturnsCorrectData()
        {
            // Arrange: создаём объект FinanceDataExport и сериализуем его в JSON
            var sampleData = new FinanceDataExport
            {
                BankAccounts = new System.Collections.Generic.List<BankAccount>
                {
                    new BankAccount(Guid.NewGuid(), "Account1", 500m)
                },
                Categories = new System.Collections.Generic.List<Category>
                {
                    new Category(Guid.NewGuid(), CategoryType.Income, "Salary")
                },
                Operations = new System.Collections.Generic.List<Operation>
                {
                    new Operation(Guid.NewGuid(), OperationType.Income, Guid.NewGuid(), 1000m, DateTime.UtcNow, "Test", Guid.NewGuid())
                }
            };
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(sampleData, options);

            // Записываем в временный файл
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, json);

            var importer = new JsonDataImporter();

            // Act
            FinanceDataExport importedData = importer.Import(tempFile);

            // Assert
            Assert.NotNull(importedData);
            Assert.Equal(sampleData.BankAccounts.Count, importedData.BankAccounts.Count);
            Assert.Equal(sampleData.Categories.Count, importedData.Categories.Count);
            Assert.Equal(sampleData.Operations.Count, importedData.Operations.Count);

            File.Delete(tempFile);
        }
    }
}
