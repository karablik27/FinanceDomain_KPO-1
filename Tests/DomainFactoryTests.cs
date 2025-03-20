using System;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class DomainFactoryTests
    {
        [Fact]
        public void CreateBankAccount_ValidData_ReturnsAccount()
        {
            // Arrange
            var factory = new DomainFactory();

            // Act
            var account = factory.CreateBankAccount("FactoryTest", 500m);

            // Assert
            Assert.Equal("FactoryTest", account.Name);
            Assert.Equal(500m, account.Balance);
        }

        [Fact]
        public void CreateCategory_ValidData_ReturnsCategory()
        {
            // Arrange
            var factory = new DomainFactory();

            // Act
            var category = factory.CreateCategory(CategoryType.Income, "Salary");

            // Assert
            Assert.Equal(CategoryType.Income, category.Type);
            Assert.Equal("Salary", category.Name);
        }

        [Fact]
        public void CreateOperation_ValidData_ReturnsOperation()
        {
            // Arrange
            var factory = new DomainFactory();
            var cat = factory.CreateCategory(CategoryType.Expense, "Food");
            var bankAccountId = Guid.NewGuid();

            // Act
            var operation = factory.CreateOperation(OperationType.Expense, bankAccountId, 100m, cat, "Lunch");

            // Assert
            Assert.Equal(OperationType.Expense, operation.Type);
            Assert.Equal(100m, operation.Amount);
            Assert.Equal(cat.Id, operation.CategoryId);
            Assert.Equal("Lunch", operation.Description);
        }

        [Fact]
        public void CreateOperation_MismatchedType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new DomainFactory();
            var cat = factory.CreateCategory(CategoryType.Expense, "Food");
            var bankAccountId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                factory.CreateOperation(OperationType.Income, bankAccountId, 100m, cat, "Mistake"));
        }
    }
}
