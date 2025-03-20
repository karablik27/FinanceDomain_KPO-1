using System;
using System.IO;
using FinanceDataImporterExporter.Import;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class CsvDataImporterTests
    {
        [Fact]
        public void Import_ValidCsv_ReturnsCorrectData()
        {
            // Arrange: создаём CSV-строку
            string csvContent =
@"BankAccount,11111111-1111-1111-1111-111111111111,Account1,500
Category,22222222-2222-2222-2222-222222222222,Income,Salary
Operation,33333333-3333-3333-3333-333333333333,Income,11111111-1111-1111-1111-111111111111,1000,2023-03-20T12:00:00Z,Monthly salary,22222222-2222-2222-2222-222222222222";
            // Записываем в временный файл
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, csvContent);

            var importer = new CsvDataImporter();

            // Act
            FinanceDataExport data = importer.Import(tempFile);

            // Assert
            Assert.NotNull(data);
            Assert.Single(data.BankAccounts);
            Assert.Single(data.Categories);
            Assert.Single(data.Operations);

            File.Delete(tempFile);
        }
    }
}
