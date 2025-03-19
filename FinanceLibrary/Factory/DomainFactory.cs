using System;

namespace FinanceLibrary
{
    public class DomainFactory : IDomainFactory
    {
        /// <summary>
        /// Создает новый банковский счет.
        /// </summary>
        public BankAccount CreateBankAccount(string name, decimal initialBalance = 0)
        {
            return new BankAccount(Guid.NewGuid(), name, initialBalance);
        }

        /// <summary>
        /// Создает новую категорию.
        /// </summary>
        public Category CreateCategory(CategoryType type, string name)
        {
            return new Category(Guid.NewGuid(), type, name);
        }

        /// <summary>
        /// Создает новую операцию.
        /// </summary>
        public Operation CreateOperation(OperationType type, Guid bankAccountId, decimal amount, Category category, string description = " ")
        {
            // Если требуется проверка соответствия типа операции и типа категории, оставляем её:
            if ((type == OperationType.Income && category.Type != CategoryType.Income) ||
                (type == OperationType.Expense && category.Type != CategoryType.Expense))
            {
                throw new ArgumentException("Тип операции должен соответствовать типу категории.");
            }

            return new Operation(Guid.NewGuid(), type, bankAccountId, amount, DateTime.Now, description, category.Id);
        }
    }
}
