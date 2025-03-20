using System;
using FinanceDataImporterExporter.Export;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class CsvExportVisitorTests
    {
        [Fact]
        public void GetResult_ReturnsValidCsv()
        {
            // Arrange
            var account = new BankAccount(Guid.NewGuid(), "Account1", 500m);
            var category = new Category(Guid.NewGuid(), CategoryType.Income, "Salary");
            var operation = new Operation(Guid.NewGuid(), OperationType.Income, account.Id, 1500m, DateTime.UtcNow, "Monthly salary", category.Id);

            var visitor = new CsvExportVisitor();
            visitor.Visit(account);
            visitor.Visit(category);
            visitor.Visit(operation);

            // Act
            string csv = visitor.GetResult();

            // Assert: CSV должен содержать строки с префиксами "BankAccount", "Category" и "Operation"
            Assert.Contains("BankAccount,", csv);
            Assert.Contains("Category,", csv);
            Assert.Contains("Operation,", csv);
        }
    }
}
