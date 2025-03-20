using System;
using Microsoft.Extensions.DependencyInjection;
using FinTech;           // IFinanceManager, RealFinanceManager, FinanceManagerProxy, ImportExportData
using FinanceLibrary;    // DomainFactory, BankAccount, Category, Operation, CategoryType, OperationType

namespace FinanceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка DI-контейнера
            var services = new ServiceCollection();
            services.AddSingleton<IDomainFactory, DomainFactory>();
            services.AddSingleton<RealFinanceManager>();
            services.AddSingleton<IFinanceManager>(provider =>
            {
                var realManager = provider.GetRequiredService<RealFinanceManager>();
                return new FinanceManagerProxy(realManager);
            });
            var serviceProvider = services.BuildServiceProvider();
            var financeManager = serviceProvider.GetRequiredService<IFinanceManager>();

            do
            {
                try
                {
                    ShowMenu();
                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            CreateBankAccount(financeManager);
                            break;
                        case "2":
                            UpdateBankAccount(financeManager);
                            break;
                        case "3":
                            DeleteBankAccount(financeManager);
                            break;
                        case "4":
                            ShowBankAccounts(financeManager);
                            break;
                        case "5":
                            CreateCategory(financeManager);
                            break;
                        case "6":
                            CreateOperation(financeManager);
                            break;
                        case "7":
                            GroupOperations(financeManager);
                            break;
                        case "8":
                            ExportDataToCsv(financeManager);
                            break;
                        case "9":
                            ExportDataToJson(financeManager);
                            break;
                        case "10":
                            ExportDataToYaml(financeManager);
                            break;
                        case "11":
                            ImportDataFromCsv(financeManager);
                            break;
                        case "12":
                            ImportDataFromJson(financeManager);
                            break;
                        case "13":
                            ImportDataFromYaml(financeManager);
                            break;
                        // Новый пункт 14: Показать все категории
                        case "14":
                            ShowCategories(financeManager);
                            break;
                        // Новый пункт 15: Показать все операции
                        case "15":
                            ShowOperations(financeManager);
                            break;
                        case "0":
                            Colors.PrintLine("Выход...", Colors.ConsoleColorType.Green);
                            return;
                        default:
                            Colors.PrintLine("Неверная опция.", Colors.ConsoleColorType.Yellow);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Colors.PrintLine($"Ошибка: {ex.Message}", Colors.ConsoleColorType.Red);
                }
                Colors.PrintLine("Нажмите любую клавишу для продолжения...", Colors.ConsoleColorType.Gray);
                Console.ReadKey();
            }
            while (true);
        }

        static void ShowMenu()
        {
            Console.Clear();
            Colors.PrintLine("=== Меню модуля 'Учет финансов' ===", Colors.ConsoleColorType.Cyan);
            Console.WriteLine("1. Создать банковский счет");
            Console.WriteLine("2. Обновить имя счета");
            Console.WriteLine("3. Удалить счет");
            Console.WriteLine("4. Показать все счета");
            Console.WriteLine("5. Создать категорию");
            Console.WriteLine("6. Создать операцию");
            Console.WriteLine("7. Группировка операций по категориям (аналитика)");
            Console.WriteLine("8. Экспорт данных в CSV");
            Console.WriteLine("9. Экспорт данных в JSON");
            Console.WriteLine("10. Экспорт данных в YAML");
            Console.WriteLine("11. Импорт данных из CSV");
            Console.WriteLine("12. Импорт данных из JSON");
            Console.WriteLine("13. Импорт данных из YAML");
            Console.WriteLine("14. Показать все категории");
            Console.WriteLine("15. Показать все операции");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
        }

        static void CreateBankAccount(IFinanceManager manager)
        {
            Colors.Print("Введите название счета: ", Colors.ConsoleColorType.White);
            string name = Console.ReadLine();
            Colors.Print("Введите начальный баланс: ", Colors.ConsoleColorType.White);
            if (decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                var account = new BankAccount(Guid.NewGuid(), name, balance);
                manager.AddBankAccount(account);
                Colors.PrintLine($"Счет создан: {account}", Colors.ConsoleColorType.Green);
            }
            else
            {
                Colors.PrintLine("Неверное значение баланса.", Colors.ConsoleColorType.Red);
            }
        }

        static void UpdateBankAccount(IFinanceManager manager)
        {
            Colors.Print("Введите ID счета для обновления: ", Colors.ConsoleColorType.White);
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Colors.Print("Введите новое имя: ", Colors.ConsoleColorType.White);
                string newName = Console.ReadLine();
                // Для обновления создаем новый объект с тем же ID (в реальном приложении лучше извлечь существующий объект)
                var updatedAccount = new BankAccount(id, newName, 0);
                manager.UpdateBankAccount(updatedAccount);
                Colors.PrintLine("Счет обновлен.", Colors.ConsoleColorType.Green);
            }
            else
            {
                Colors.PrintLine("Неверный формат ID.", Colors.ConsoleColorType.Red);
            }
        }

        static void DeleteBankAccount(IFinanceManager manager)
        {
            Colors.Print("Введите ID счета для удаления: ", Colors.ConsoleColorType.White);
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                manager.RemoveBankAccount(id);
                Colors.PrintLine("Счет удален.", Colors.ConsoleColorType.Green);
            }
            else
            {
                Colors.PrintLine("Неверный формат ID.", Colors.ConsoleColorType.Red);
            }
        }

        static void ShowBankAccounts(IFinanceManager manager)
        {
            Colors.PrintLine("Список банковских счетов:", Colors.ConsoleColorType.Cyan);
            foreach (var account in manager.GetBankAccounts())
            {
                Console.WriteLine(account);
            }
        }

        static void CreateCategory(IFinanceManager manager)
        {
            Colors.Print("Введите название категории: ", Colors.ConsoleColorType.White);
            string name = Console.ReadLine();
            Colors.Print("Введите тип категории (Income/Expense): ", Colors.ConsoleColorType.White);
            string typeStr = Console.ReadLine();
            if (Enum.TryParse(typeStr, true, out CategoryType type))
            {
                var category = new Category(Guid.NewGuid(), type, name);
                manager.AddCategory(category);
                Colors.PrintLine($"Категория создана: {category}", Colors.ConsoleColorType.Green);
            }
            else
            {
                Colors.PrintLine("Неверный тип категории.", Colors.ConsoleColorType.Red);
            }
        }

        static void CreateOperation(IFinanceManager manager)
        {
            Colors.Print("Введите ID счета для операции: ", Colors.ConsoleColorType.White);
            if (!Guid.TryParse(Console.ReadLine(), out Guid accountId))
            {
                Colors.PrintLine("Неверный формат ID счета.", Colors.ConsoleColorType.Red);
                return;
            }
            Colors.Print("Введите сумму операции: ", Colors.ConsoleColorType.White);
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Colors.PrintLine("Неверное значение суммы.", Colors.ConsoleColorType.Red);
                return;
            }
            Colors.Print("Введите тип операции (Income/Expense): ", Colors.ConsoleColorType.White);
            string typeStr = Console.ReadLine();
            if (!Enum.TryParse(typeStr, true, out OperationType opType))
            {
                Colors.PrintLine("Неверный тип операции.", Colors.ConsoleColorType.Red);
                return;
            }
            Colors.Print("Введите ID категории для операции: ", Colors.ConsoleColorType.White);
            if (!Guid.TryParse(Console.ReadLine(), out Guid categoryId))
            {
                Colors.PrintLine("Неверный формат ID категории.", Colors.ConsoleColorType.Red);
                return;
            }
            Colors.Print("Введите описание операции: ", Colors.ConsoleColorType.White);
            string description = Console.ReadLine();

            // Создаем объект категории для демонстрации (на практике выбирают из списка)
            var category = new Category(categoryId, opType == OperationType.Income ? CategoryType.Income : CategoryType.Expense, "Пример категории");
            var operation = new Operation(Guid.NewGuid(), opType, accountId, amount, DateTime.Now, description, category.Id);
            manager.AddOperation(operation);
            Colors.PrintLine($"Операция создана: {operation}", Colors.ConsoleColorType.Green);
        }

        static void GroupOperations(IFinanceManager manager)
        {
            Colors.Print("Введите дату начала (yyyy-MM-dd): ", Colors.ConsoleColorType.White);
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
            {
                Colors.PrintLine("Неверный формат даты.", Colors.ConsoleColorType.Red);
                return;
            }
            Colors.Print("Введите дату конца (yyyy-MM-dd): ", Colors.ConsoleColorType.White);
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
            {
                Colors.PrintLine("Неверный формат даты.", Colors.ConsoleColorType.Red);
                return;
            }
            var groups = manager.GroupOperationsByCategory(start, end);
            Colors.PrintLine("Группировка операций по категориям:", Colors.ConsoleColorType.Cyan);
            foreach (var group in groups)
            {
                Console.WriteLine($"Категория ID {group.Key}: Сумма {group.Value}");
            }
        }

        static void ExportDataToCsv(IFinanceManager manager)
        {
            Colors.Print("Введите директорию для экспорта CSV: ", Colors.ConsoleColorType.White);
            string dir = Console.ReadLine();
            try
            {
                manager.ExportToCsv(dir);
                Colors.PrintLine("Экспорт в CSV выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка экспорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        static void ExportDataToJson(IFinanceManager manager)
        {
            Colors.Print("Введите путь к файлу для экспорта JSON: ", Colors.ConsoleColorType.White);
            string path = Console.ReadLine();
            try
            {
                manager.ExportToJson(path);
                Colors.PrintLine("Экспорт в JSON выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка экспорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        static void ExportDataToYaml(IFinanceManager manager)
        {
            Colors.Print("Введите путь к файлу для экспорта YAML: ", Colors.ConsoleColorType.White);
            string path = Console.ReadLine();
            try
            {
                manager.ExportToYaml(path);
                Colors.PrintLine("Экспорт в YAML выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка экспорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        static void ImportDataFromCsv(IFinanceManager manager)
        {
            Colors.Print("Введите директорию для импорта CSV: ", Colors.ConsoleColorType.White);
            string dir = Console.ReadLine();
            try
            {
                manager.ImportFromCsv(dir);
                Colors.PrintLine("Импорт из CSV выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка импорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        static void ImportDataFromJson(IFinanceManager manager)
        {
            Colors.Print("Введите путь к файлу для импорта JSON: ", Colors.ConsoleColorType.White);
            string path = Console.ReadLine();
            try
            {
                manager.ImportFromJson(path);
                Colors.PrintLine("Импорт из JSON выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка импорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        static void ImportDataFromYaml(IFinanceManager manager)
        {
            Colors.Print("Введите путь к файлу для импорта YAML: ", Colors.ConsoleColorType.White);
            string path = Console.ReadLine();
            try
            {
                manager.ImportFromYaml(path);
                Colors.PrintLine("Импорт из YAML выполнен.", Colors.ConsoleColorType.Green);
            }
            catch (Exception ex)
            {
                Colors.PrintLine($"Ошибка импорта: {ex.Message}", Colors.ConsoleColorType.Red);
            }
        }

        // Показать все категории
        static void ShowCategories(IFinanceManager manager)
        {
            Colors.PrintLine("Список категорий:", Colors.ConsoleColorType.Cyan);
            foreach (var category in manager.GetCategories())
            {
                Console.WriteLine(category);
            }
        }

        // Показать все операции
        static void ShowOperations(IFinanceManager manager)
        {
            Colors.PrintLine("Список операций:", Colors.ConsoleColorType.Cyan);
            foreach (var operation in manager.GetOperations())
            {
                Console.WriteLine(operation);
            }
        }
    }
}
