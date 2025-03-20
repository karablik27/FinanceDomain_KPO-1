using System;
using System.Linq;
using FinanceLibrary;
using FinanceFacade.Interfaces;
using FinTech;
using Xunit;

namespace FinanceTests
{
    public class FinanceManagerProxyTests
    {
        [Fact]
        public void Proxy_InitializesCacheOnCreation()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var account = new BankAccount("Initial Account", 100m);
            realManager.AddBankAccount(account);

            // Act
            var proxy = new FinanceManagerProxy(realManager);
            var cachedAccounts = proxy.GetBankAccounts().ToList();

            // Assert – при создании прокси кэш должен содержать добавленный аккаунт
            Assert.Single(cachedAccounts);
            Assert.Equal(account.Id, cachedAccounts[0].Id);
        }

        [Fact]
        public void Proxy_AddBankAccount_RefreshesCache()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var proxy = new FinanceManagerProxy(realManager);

            // Act – добавляем новый счет через прокси
            var newAccount = new BankAccount("New Account", 200m);
            proxy.AddBankAccount(newAccount);

            // Assert – кэш должен обновиться и содержать новый счет
            var cachedAccounts = proxy.GetBankAccounts().ToList();
            Assert.Single(cachedAccounts);
            Assert.Equal("New Account", cachedAccounts[0].Name);
            Assert.Equal(200m, cachedAccounts[0].Balance);
        }

        [Fact]
        public void Proxy_UpdateBankAccount_RefreshesCache()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var account = new BankAccount("Old Name", 150m);
            realManager.AddBankAccount(account);
            var proxy = new FinanceManagerProxy(realManager);

            // Act – обновляем имя счета через прокси
            account.UpdateName("Updated Name");
            proxy.UpdateBankAccount(account);

            // Assert – кэш должен содержать обновленный счет
            var updatedAccount = proxy.GetBankAccounts().First(a => a.Id == account.Id);
            Assert.Equal("Updated Name", updatedAccount.Name);
        }

        [Fact]
        public void Proxy_RemoveBankAccount_RefreshesCache()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var account = new BankAccount("Account to Remove", 300m);
            realManager.AddBankAccount(account);
            var proxy = new FinanceManagerProxy(realManager);

            // Act – удаляем счет через прокси
            proxy.RemoveBankAccount(account.Id);

            // Assert – кэш не должен содержать удалённый счет
            var cachedAccounts = proxy.GetBankAccounts().ToList();
            Assert.DoesNotContain(cachedAccounts, a => a.Id == account.Id);
        }

        [Fact]
        public void Proxy_AddCategory_RefreshesCache()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var proxy = new FinanceManagerProxy(realManager);

            // Act – добавляем категорию через прокси
            var category = new Category("Salary", CategoryType.Income);
            proxy.AddCategory(category);

            // Assert – кэш должен содержать добавленную категорию
            var cachedCategories = proxy.GetCategories().ToList();
            Assert.Single(cachedCategories);
            Assert.Equal("Salary", cachedCategories[0].Name);
            Assert.Equal(CategoryType.Income, cachedCategories[0].Type);
        }

        [Fact]
        public void Proxy_AddOperation_RefreshesCache()
        {
            // Arrange
            var realManager = new RealFinanceManager();
            var proxy = new FinanceManagerProxy(realManager);

            // Добавляем необходимые объекты
            var category = new Category("Salary", CategoryType.Income);
            proxy.AddCategory(category);
            var account = new BankAccount("Account", 500m);
            proxy.AddBankAccount(account);

            // Act – добавляем операцию через прокси
            var operation = new Operation(OperationType.Income, account.Id, 100m, DateTime.Now, "Deposit", category.Id);
            proxy.AddOperation(operation);

            // Assert – кэш должен содержать добавленную операцию
            var cachedOperations = proxy.GetOperations().ToList();
            Assert.Single(cachedOperations);
            Assert.Equal("Deposit", cachedOperations[0].Description);
        }
    }
}
