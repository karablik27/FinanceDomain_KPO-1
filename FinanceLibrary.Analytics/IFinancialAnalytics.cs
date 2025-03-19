public interface IFinancialAnalytics
{
    decimal CalculateBalanceForPeriod(DateTime start, DateTime end);
    IDictionary<Category, decimal> GroupExpensesByCategory(DateTime start, DateTime end);
    IDictionary<Category, decimal> GroupIncomeByCategory(DateTime start, DateTime end);
    // Дополнительная аналитика...
} 