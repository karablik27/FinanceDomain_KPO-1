using System;
using System.Collections.Generic;
using System.Linq;
using FinanceLibrary;
using FinanceLibrary.Facades.Implementations;
using FinanceFacade.Interfaces;
using Xunit;

namespace FinanceTests
{
    public class BankAccountFacadeTests
    {
        [Fact]
        public void Create_ValidData_CreatesAccount()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IBankAccountFacade facade = new BankAccountFacade((DomainFactory)factory);

            // Act
            var account = facade.Create("Test", 1000m);

            // Assert
            Assert.NotNull(account);
            Assert.Equal("Test", account.Name);
            Assert.Equal(1000m, account.Balance);

            // Проверяем, что счет добавлен во внутреннюю коллекцию
            var accounts = facade.GetAll().ToList();
            Assert.Single(accounts);
            Assert.Contains(account, accounts);
        }

        [Fact]
        public void Update_ValidId_UpdatesName()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IBankAccountFacade facade = new BankAccountFacade((DomainFactory)factory);
            var account = facade.Create("OldName", 200m);

            // Act
            facade.Update(account.Id, "NewName");

            // Assert – получаем обновленный счет
            var updatedAccount = facade.GetById(account.Id);
            Assert.Equal("NewName", updatedAccount.Name);
        }

        [Fact]
        public void Delete_ValidId_RemovesAccount()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IBankAccountFacade facade = new BankAccountFacade((DomainFactory)factory);
            var account = facade.Create("DeleteMe", 50m);

            // Act
            facade.Delete(account.Id);

            // Assert – проверяем, что счета больше нет в коллекции
            var accounts = facade.GetAll().ToList();
            Assert.DoesNotContain(account, accounts);
        }

        [Fact]
        public void GetById_InvalidId_ThrowsKeyNotFound()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            IBankAccountFacade facade = new BankAccountFacade((DomainFactory)factory);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => facade.GetById(Guid.NewGuid()));
        }
    }
}
