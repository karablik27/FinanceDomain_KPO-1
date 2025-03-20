using System;
using FinanceLibrary;
using Xunit;

namespace FinanceTests
{
    public class CategoryTests
    {
        [Fact]
        public void Ctor_ValidData_InitializesProperties()
        {
            // Arrange
            var type = CategoryType.Expense;
            var name = "Food";

            // Act
            var category = new Category(name, type);

            // Assert
            Assert.Equal(type, category.Type);
            Assert.Equal(name, category.Name);
            Assert.NotEqual(Guid.Empty, category.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Ctor_InvalidName_ThrowsArgumentException(string invalidName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Category(invalidName, CategoryType.Income));
        }

        [Fact]
        public void UpdateName_Valid_ChangesName()
        {
            // Arrange
            var category = new Category("OldName", CategoryType.Expense);

            // Act
            category.UpdateName("NewName");

            // Assert
            Assert.Equal("NewName", category.Name);
        }

        [Fact]
        public void UpdateName_Invalid_ThrowsArgumentException()
        {
            // Arrange
            var category = new Category("Test", CategoryType.Income);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => category.UpdateName(""));
        }
    }
}
