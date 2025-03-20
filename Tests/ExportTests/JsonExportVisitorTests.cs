using System;
using System.Collections.Generic;
using System.Text.Json;
using FinanceDataImporterExporter.Export;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class JsonExportVisitorTests
    {
        [Fact]
        public void GetResult_ReturnsValidJson()
        {
            // Arrange: создаём несколько объектов
            var account = new BankAccount(Guid.NewGuid(), "Account1", 500m);
            var category = new Category(Guid.NewGuid(), CategoryType.Expense, "Food");
            var operation = new Operation(Guid.NewGuid(), OperationType.Expense, account.Id, 100m, DateTime.UtcNow, "Dinner", category.Id);

            var visitor = new JsonExportVisitor();
            visitor.Visit(account);
            visitor.Visit(category);
            visitor.Visit(operation);

            // Act
            string json = visitor.GetResult();

            // Assert: пытаемся десериализовать полученный JSON
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("BankAccounts"));
            Assert.True(result.ContainsKey("Categories"));
            Assert.True(result.ContainsKey("Operations"));
        }
    }
}
