public interface IFinancialDataRepository
{
    void Save(FinancialData data);
    FinancialData Load();
}

public class CacheProxyRepository : IFinancialDataRepository
{
    private readonly IFinancialDataRepository _realRepository;
    private FinancialData _cachedData;

    public void Save(FinancialData data)
    {
        _realRepository.Save(data);
    }

    public FinancialData Load()
    {
        if (_cachedData == null)
        {
            _cachedData = _realRepository.Load();
        }
        return _cachedData;
    }
} 