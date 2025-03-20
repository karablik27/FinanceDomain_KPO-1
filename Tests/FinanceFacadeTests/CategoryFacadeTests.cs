using System;
using System.Linq;
using FinanceLibrary;
using FinanceLibrary.Facades.Implementations;
using FinanceFacade.Interfaces;
using Xunit;

namespace FinanceTests
{
    public class CategoryFacadeTests
    {
        [Fact]
        public void Create_ValidData_CreatesCategory()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            ICategoryFacade facade = new CategoryFacade((DomainFactory)factory);

            // Act
            var category = facade.Create(CategoryType.Income, "Salary");

            // Assert
            Assert.NotNull(category);
            Assert.Equal("Salary", category.Name);
            Assert.Equal(CategoryType.Income, category.Type);
            Assert.Single(facade.GetAll());
        }

        [Fact]
        public void Update_ValidId_UpdatesName()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            ICategoryFacade facade = new CategoryFacade((DomainFactory)factory);
            var category = facade.Create(CategoryType.Expense, "Food");

            // Act
            facade.Update(category.Id, "Groceries");

            // Assert
            var updatedCategory = facade.GetById(category.Id);
            Assert.Equal("Groceries", updatedCategory.Name);
        }

        [Fact]
        public void Delete_ValidId_RemovesCategory()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            ICategoryFacade facade = new CategoryFacade((DomainFactory)factory);
            var category = facade.Create(CategoryType.Expense, "Transport");

            // Act
            facade.Delete(category.Id);

            // Assert
            var categories = facade.GetAll().ToList();
            Assert.DoesNotContain(category, categories);
        }

        [Fact]
        public void GetById_InvalidId_ThrowsKeyNotFoundException()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            ICategoryFacade facade = new CategoryFacade((DomainFactory)factory);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => facade.GetById(Guid.NewGuid()));
        }

        [Fact]
        public void GetByType_ReturnsOnlyMatchingCategories()
        {
            // Arrange
            IDomainFactory factory = new DomainFactory();
            ICategoryFacade facade = new CategoryFacade((DomainFactory)factory);
            var cat1 = facade.Create(CategoryType.Income, "Salary");
            var cat2 = facade.Create(CategoryType.Expense, "Food");
            var cat3 = facade.Create(CategoryType.Expense, "Transport");

            // Act
            var expenseCategories = facade.GetByType(CategoryType.Expense).ToList();

            // Assert
            Assert.Equal(2, expenseCategories.Count);
            Assert.Contains(expenseCategories, c => c.Name == "Food");
            Assert.Contains(expenseCategories, c => c.Name == "Transport");
        }
    }
}
