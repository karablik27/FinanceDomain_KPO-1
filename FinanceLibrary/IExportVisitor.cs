using FinanceLibrary;

namespace FinanceDataImporterExporter.Export
{
    /// <summary>
    /// Интерфейс посетителя для экспорта доменных объектов.
    /// </summary>
    public interface IExportVisitor
    {
        void Visit(BankAccount bankAccount);
        void Visit(Category category);
        void Visit(Operation operation);
    }
}
