using System;

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

        public Category(Guid id, CategoryType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
            Id = id;
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Обновляет имя категории.
        /// </summary>
        /// <param name="newName">Новое имя категории.</param>
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
