using System;
using System.Text.Json.Serialization;

namespace FinanceLibrary
{
    public enum CategoryType
    {
        Income,
        Expense
    }

    public class Category
    {
        public Guid Id { get; private set; }
        public CategoryType Type { get; private set; }
        public string Name { get; private set; }

        // Основной конструктор для создания новой категории
        public Category(string name, CategoryType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название категории не может быть пустым.", nameof(name));

            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }

        // Конструктор для десериализации
        [JsonConstructor]
        public Category(Guid id, CategoryType type, string name)
            : this(name, type)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID категории не может быть пустым.", nameof(id));
            Id = id;
        }

        /// <summary>
        /// Обновляет имя категории.
        /// </summary>
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Новое имя не может быть пустым.", nameof(newName));
            Name = newName;
        }

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }

        public void Accept(FinanceDataImporterExporter.Export.IExportVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
