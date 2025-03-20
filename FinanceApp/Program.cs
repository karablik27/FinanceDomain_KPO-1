using Microsoft.Extensions.DependencyInjection;
using FinanceLibrary;
using FinanceLibrary.Facades.Implementations;
using FinanceFacade.Interfaces;
using FinTech;

namespace FinanceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Настройка DI-контейнера
                var services = new ServiceCollection();

                // Регистрируем фабрику через интерфейс
                services.AddSingleton<IDomainFactory, DomainFactory>();
                services.AddSingleton<RealFinanceManager>();

                // Регистрируем фасады, используя зависимость IDomainFactory
                services.AddSingleton<IBankAccountFacade, BankAccountFacade>();
                services.AddSingleton<ICategoryFacade, CategoryFacade>();
                services.AddSingleton<IOperationFacade, OperationFacade>();

                // Регистрируем IFinanceManager как прокси для кэширования
                services.AddSingleton<IFinanceManager>(provider =>
                {
                    var realManager = provider.GetRequiredService<RealFinanceManager>();
                    return new FinanceManagerProxy(realManager);
                });

                var serviceProvider = services.BuildServiceProvider();

                // Получаем фасады для создания команд
                var bankAccountFacade = serviceProvider.GetRequiredService<IBankAccountFacade>();
                var categoryFacade = serviceProvider.GetRequiredService<ICategoryFacade>();
                var operationFacade = serviceProvider.GetRequiredService<IOperationFacade>();

                // Получаем общий менеджер (прокси)
                var financeManager = serviceProvider.GetRequiredService<IFinanceManager>();

                // Вызов меню и выполнение операций вынесены в отдельный класс Methods
                Methods.ShowMenuAndExecute(financeManager, bankAccountFacade, categoryFacade, operationFacade);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Критическая ошибка: " + ex.Message);
            }
        }
    }
}
