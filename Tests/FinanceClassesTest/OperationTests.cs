using System;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class OperationTests
    {
        [Fact]
        public void Ctor_ValidData_InitializesProperties()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var date = DateTime.Now;

            // Act
            var op = new Operation(OperationType.Income, accountId, 500m, date, "Test Operation", categoryId);

            // Assert
            Assert.Equal(OperationType.Income, op.Type);
            Assert.Equal(accountId, op.BankAccountId);
            Assert.Equal(500m, op.Amount);
            Assert.Equal(date, op.Date);
            Assert.Equal("Test Operation", op.Description);
            Assert.Equal(categoryId, op.CategoryId);
        }

        [Fact]
        public void Ctor_InvalidBankAccountId_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Operation(OperationType.Expense, Guid.Empty, 100m, DateTime.Now, "Desc", Guid.NewGuid()));
        }

        [Fact]
        public void Ctor_InvalidCategoryId_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Operation(OperationType.Expense, Guid.NewGuid(), 100m, DateTime.Now, "Desc", Guid.Empty));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Ctor_InvalidAmount_Throws(decimal amount)
        {
            Assert.Throws<ArgumentException>(() =>
                new Operation(OperationType.Income, Guid.NewGuid(), amount, DateTime.Now, null, Guid.NewGuid()));
        }

        [Fact]
        public void Ctor_FutureDate_Throws()
        {
            var futureDate = DateTime.Now.AddDays(1);
            Assert.Throws<ArgumentException>(() =>
                new Operation(OperationType.Income, Guid.NewGuid(), 100m, futureDate, "", Guid.NewGuid()));
        }

        [Fact]
        public void Ctor_DescriptionIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Operation(OperationType.Income, Guid.NewGuid(), 100m, DateTime.Now, "   ", Guid.NewGuid()));
        }
    }
}
