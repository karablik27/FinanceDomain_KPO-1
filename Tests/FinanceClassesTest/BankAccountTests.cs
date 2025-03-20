using System;
using System.Collections.Generic;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class BankAccountTests
    {
        [Fact]
        public void Ctor_ValidData_InitializesProperties()
        {
            // Arrange
            var name = "Main Account";
            decimal balance = 1000m;

            // Act
            var account = new BankAccount(name, balance);

            // Assert
            Assert.Equal(name, account.Name);
            Assert.Equal(balance, account.Balance);
            Assert.NotEqual(Guid.Empty, account.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Ctor_InvalidName_ThrowsArgumentException(string invalidName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new BankAccount(invalidName, 100));
        }

        [Fact]
        public void Ctor_NegativeBalance_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new BankAccount("Test", -1));
        }

        [Fact]
        public void Deposit_ValidAmount_IncreasesBalance()
        {
            // Arrange
            var account = new BankAccount("Test", 500m);

            // Act
            account.Deposit(200m);

            // Assert
            Assert.Equal(700m, account.Balance);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Deposit_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var account = new BankAccount("Test", 500m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => account.Deposit(amount));
        }

        [Fact]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            // Arrange
            var account = new BankAccount("Test", 500m);

            // Act
            account.Withdraw(300m);

            // Assert
            Assert.Equal(200m, account.Balance);
        }

        [Fact]
        public void Withdraw_TooLarge_ThrowsInvalidOperationException()
        {
            // Arrange
            var account = new BankAccount("Test", 200m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(300m));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Withdraw_InvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var account = new BankAccount("Test", 200m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => account.Withdraw(amount));
        }

        [Fact]
        public void UpdateName_ValidName_ChangesName()
        {
            // Arrange
            var account = new BankAccount("OldName", 100m);

            // Act
            account.UpdateName("NewName");

            // Assert
            Assert.Equal("NewName", account.Name);
        }

        [Fact]
        public void UpdateName_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var account = new BankAccount("OldName", 100m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => account.UpdateName(""));
        }

        [Fact]
        public void RecalculateBalance_ValidOperations_SumsCorrectly()
        {
            // Arrange
            var account = new BankAccount("Test", 0m);
            var ops = new List<Operation>
            {
                new Operation(OperationType.Income, account.Id, 500m, DateTime.Now, "Income", Guid.NewGuid()),
                new Operation(OperationType.Expense, account.Id, 200m, DateTime.Now, "Expense", Guid.NewGuid()),
                new Operation(OperationType.Income, account.Id, 300m, DateTime.Now, "Income2", Guid.NewGuid())
            };

            // Act
            account.RecalculateBalance(ops);

            // Assert
            Assert.Equal(600m, account.Balance); // 500 - 200 + 300 = 600
        }
    }
}
