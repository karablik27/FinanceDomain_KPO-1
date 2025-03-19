public abstract class DataExporter
{
    protected abstract void WriteData(FinancialData data, string path);
    
    public void Export(FinancialData data, string path)
    {
        // Общая логика экспорта
        ValidatePath(path);
        WriteData(data, path);
    }
} 