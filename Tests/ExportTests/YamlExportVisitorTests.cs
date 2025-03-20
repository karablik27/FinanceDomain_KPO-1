using System;
using FinanceDataImporterExporter.Export;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class YamlExportVisitorTests
    {
        [Fact]
        public void GetResult_ReturnsValidYaml()
        {
            // Arrange
            var account = new BankAccount(Guid.NewGuid(), "Account1", 500m);
            var category = new Category(Guid.NewGuid(), CategoryType.Expense, "Food");
            var operation = new Operation(Guid.NewGuid(), OperationType.Expense, account.Id, 100m, DateTime.UtcNow, "Dinner", category.Id);

            var visitor = new YamlExportVisitor();
            visitor.Visit(account);
            visitor.Visit(category);
            visitor.Visit(operation);

            // Act
            string yaml = visitor.GetResult();

            // Assert: YAML должен содержать ключи "bankAccounts", "categories" и "operations"
            Assert.Contains("bankAccounts:", yaml);
            Assert.Contains("categories:", yaml);
            Assert.Contains("operations:", yaml);
        }
    }
}
