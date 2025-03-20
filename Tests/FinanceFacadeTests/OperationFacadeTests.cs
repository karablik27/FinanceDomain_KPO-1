using System;
using System.Linq;
using FinanceLibrary;
using FinanceLibrary.Facades.Implementations;
using FinanceFacade.Interfaces;
using Xunit;

namespace FinanceTests
{
    public class OperationFacadeTests
    {
        [Fact]
        public void Create_ValidData_CreatesOperation()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);
            // Для создания операции нужен объект категории
            var category = new Category("TestCategory", CategoryType.Income);
            var accountId = Guid.NewGuid();

            // Act
            var operation = facade.Create(OperationType.Income, accountId, 200m, category, "Test Operation");

            // Assert
            Assert.NotNull(operation);
            Assert.Equal(OperationType.Income, operation.Type);
            Assert.Equal(accountId, operation.BankAccountId);
            Assert.Equal(200m, operation.Amount);
            Assert.Equal("Test Operation", operation.Description);
            Assert.Equal(category.Id, operation.CategoryId);
        }

        [Fact]
        public void Delete_ValidId_RemovesOperation()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);
            var category = new Category("TestCategory", CategoryType.Expense);
            var accountId = Guid.NewGuid();
            var operation = facade.Create(OperationType.Expense, accountId, 100m, category, "Test");

            // Act
            facade.Delete(operation.Id);

            // Assert
            var operations = facade.GetAll().ToList();
            Assert.DoesNotContain(operation, operations);
        }

        [Fact]
        public void GetById_InvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => facade.GetById(Guid.NewGuid()));
        }

        [Fact]
        public void GetByAccount_ReturnsOnlyMatchingOperations()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);
            var category = new Category("Category", CategoryType.Income);
            var accountId = Guid.NewGuid();
            var op1 = facade.Create(OperationType.Income, accountId, 150m, category, "Op1");
            var op2 = facade.Create(OperationType.Income, accountId, 250m, category, "Op2");
            var op3 = facade.Create(OperationType.Income, Guid.NewGuid(), 300m, category, "Op3");

            // Act
            var operationsForAccount = facade.GetByAccount(accountId).ToList();

            // Assert
            Assert.Equal(2, operationsForAccount.Count);
            Assert.Contains(operationsForAccount, o => o.Id == op1.Id);
            Assert.Contains(operationsForAccount, o => o.Id == op2.Id);
        }

        [Fact]
        public void GetByCategory_ReturnsOnlyMatchingOperations()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);
            var category1 = new Category("Category1", CategoryType.Income);
            var category2 = new Category("Category2", CategoryType.Income);
            var accountId = Guid.NewGuid();
            var op1 = facade.Create(OperationType.Income, accountId, 100m, category1, "Op1");
            var op2 = facade.Create(OperationType.Income, accountId, 200m, category1, "Op2");
            var op3 = facade.Create(OperationType.Income, accountId, 300m, category2, "Op3");

            // Act
            var opsCat1 = facade.GetByCategory(category1.Id).ToList();

            // Assert
            Assert.Equal(2, opsCat1.Count);
            Assert.Contains(opsCat1, o => o.Id == op1.Id);
            Assert.Contains(opsCat1, o => o.Id == op2.Id);
        }

        [Fact]
        public void GetByDateRange_ReturnsOnlyMatchingOperations()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IOperationFacade facade = new OperationFacade((DomainFactory)factory);
            var category = new Category("Category", CategoryType.Income);
            var accountId = Guid.NewGuid();
            DateTime now = DateTime.Now;
            var op1 = facade.Create(OperationType.Income, accountId, 100m, category, "Op1");
            var op2 = facade.Create(OperationType.Income, accountId, 200m, category, "Op2");
            // Допустим, операции создаются с текущей датой
            // Act
            var opsInRange = facade.GetByDateRange(now.AddMinutes(-1), now.AddMinutes(1)).ToList();

            // Assert
            Assert.True(opsInRange.Count >= 2);
        }
    }
}
