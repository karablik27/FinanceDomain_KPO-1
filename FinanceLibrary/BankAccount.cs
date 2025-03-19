using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceLibrary
{
    public class BankAccount
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public BankAccount(Guid id, string name, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название счета не может быть пустым.", nameof(name));
            if (initialBalance < 0)
                throw new ArgumentException("Начальный баланс не может быть отрицательным.", nameof(initialBalance));
            Id = id;
            Name = name;
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма пополнения должна быть больше нуля.", nameof(amount));
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма списания должна быть больше нуля.", nameof(amount));
            if (amount > Balance)
                throw new InvalidOperationException("Недостаточно средств на счете для выполнения операции.");
            Balance -= amount;
        }

        /// <summary>
        /// Обновляет имя счета.
        /// </summary>
        /// <param name="newName">Новое имя счета.</param>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Новое имя не может быть пустым.", nameof(newName));
            Name = newName;
        }

        /// <summary>
        /// Пересчитывает баланс на основе списка операций.
        /// </summary>
        /// <param name="operations">Набор операций, относящихся к этому счету.</param>
        public void RecalculateBalance(IEnumerable<Operation> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));
            Balance = operations
                .Where(op => op.BankAccountId == this.Id)
                .Sum(op => op.Type == OperationType.Income ? op.Amount : -op.Amount);
        }

        public override string ToString()
        {
            return $"Счет: {Name} (ID: {Id}) - Баланс: {Balance:C}";
        }

        public void Accept(FinanceDataImporterExporter.Export.IExportVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
