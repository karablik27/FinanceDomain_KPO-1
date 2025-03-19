using System;
using System.Text.Json.Serialization;

namespace FinanceLibrary
{
    public enum OperationType
    {
        Income,
        Expense
    }

    public class Operation
    {
        public Guid Id { get; private set; }
        public OperationType Type { get; private set; }
        public Guid BankAccountId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public Guid CategoryId { get; private set; }

        /// <summary>
        /// Основной конструктор для создания новой операции.
        /// Генерирует новый ID и выполняет валидацию остальных параметров.
        /// </summary>
        public Operation(OperationType type, Guid bankAccountId, decimal amount, DateTime date, string description, Guid categoryId)
        {
            if (bankAccountId == Guid.Empty)
                throw new ArgumentException("Идентификатор счета не может быть пустым.", nameof(bankAccountId));
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Идентификатор категории не может быть пустым.", nameof(categoryId));
            if (amount <= 0)
                throw new ArgumentException("Сумма операции должна быть больше нуля.", nameof(amount));
            if (date == default(DateTime))
                throw new ArgumentException("Дата операции не может быть значением по умолчанию.", nameof(date));
            if (date > DateTime.Now)
                throw new ArgumentException("Дата операции не может быть в будущем.", nameof(date));
            if (description != null && string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Если указано описание, оно не может быть пустой строкой.", nameof(description));

            Id = Guid.NewGuid();
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }

        /// <summary>
        /// Конструктор для десериализации.
        /// Позволяет установить ID из входных данных (например, из JSON).
        /// </summary>
        [JsonConstructor]
        public Operation(Guid id, OperationType type, Guid bankAccountId, decimal amount, DateTime date, string description, Guid categoryId)
            : this(type, bankAccountId, amount, date, description, categoryId)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID операции не может быть пустым.", nameof(id));

            // Заменяем сгенерированный ID на переданный
            Id = id;
        }

        public override string ToString()
        {
            return $"{Type} операция: {Amount:C} от {Date:d}";
        }

        public void Accept(FinanceDataImporterExporter.Export.IExportVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

