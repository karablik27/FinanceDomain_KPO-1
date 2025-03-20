using System;
using FinanceFacade.Interfaces;
using FinanceLibrary;
using FinTech;

namespace FinanceApp
{
    public static class Methods
    {
        public static void ShowMenuAndExecute(IFinanceManager financeManager,
                                                IBankAccountFacade bankAccountFacade,
                                                ICategoryFacade categoryFacade,
                                                IOperationFacade operationFacade)
        {
            do
            {
                try
                {
                    ShowMenu();
                    string option = Console.ReadLine()?.Trim();
                    switch (option)
                    {
                        case "1":
                            CreateBankAccountCommandExample(bankAccountFacade);
                            break;
                        case "2":
                            UpdateBankAccountCommandExample(bankAccountFacade);
                            break;
                        case "3":
                            DeleteBankAccountCommandExample(bankAccountFacade);
                            break;
                        case "4":
                            ShowBankAccounts(financeManager);
                            break;
                        case "5":
                            CreateCategoryCommandExample(categoryFacade);
                            break;
                        case "6":
                            CreateOperationCommandExample(operationFacade, categoryFacade);
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
                        case "14":
                            ShowCategories(financeManager);
                            break;
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
            Console.WriteLine("1. Создать банковский счет (через команду)");
            Console.WriteLine("2. Обновить имя счета (через команду)");
            Console.WriteLine("3. Удалить счет (через команду)");
            Console.WriteLine("4. Показать все счета");
            Console.WriteLine("5. Создать категорию (через команду)");
            Console.WriteLine("6. Создать операцию (через команду)");
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

        static void CreateBankAccountCommandExample(IBankAccountFacade bankAccountFacade)
        {
            Colors.Print("Введите название счета: ", Colors.ConsoleColorType.White);
            string name = Console.ReadLine();
            Colors.Print("Введите начальный баланс: ", Colors.ConsoleColorType.White);
            if (decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                ICommand command = new CreateBankAccountCommand(bankAccountFacade, name, balance);
                ICommand timedCommand = new TimedCommandDecorator(command);
                timedCommand.Execute();
            }
            else
            {
                Colors.PrintLine("Неверное значение баланса.", Colors.ConsoleColorType.Red);
            }
        }

        static void UpdateBankAccountCommandExample(IBankAccountFacade bankAccountFacade)
        {
            Colors.Print("Введите ID счета для обновления: ", Colors.ConsoleColorType.White);
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Colors.Print("Введите новое имя: ", Colors.ConsoleColorType.White);
                string newName = Console.ReadLine();
                ICommand command = new UpdateBankAccountCommand(bankAccountFacade, id, newName);
                ICommand timedCommand = new TimedCommandDecorator(command);
                timedCommand.Execute();
            }
            else
            {
                Colors.PrintLine("Неверный формат ID.", Colors.ConsoleColorType.Red);
            }
        }

        static void DeleteBankAccountCommandExample(IBankAccountFacade bankAccountFacade)
        {
            Colors.Print("Введите ID счета для удаления: ", Colors.ConsoleColorType.White);
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                ICommand command = new DeleteBankAccountCommand(bankAccountFacade, id);
                ICommand timedCommand = new TimedCommandDecorator(command);
                timedCommand.Execute();
            }
            else
            {
                Colors.PrintLine("Неверный формат ID.", Colors.ConsoleColorType.Red);
            }
        }

        static void CreateCategoryCommandExample(ICategoryFacade categoryFacade)
        {
            Colors.Print("Введите название категории: ", Colors.ConsoleColorType.White);
            string name = Console.ReadLine();
            Colors.Print("Введите тип категории (Income/Expense): ", Colors.ConsoleColorType.White);
            string typeStr = Console.ReadLine();
            if (Enum.TryParse(typeStr, true, out CategoryType type))
            {
                ICommand command = new CreateCategoryCommand(categoryFacade, type, name);
                ICommand timedCommand = new TimedCommandDecorator(command);
                timedCommand.Execute();
            }
            else
            {
                Colors.PrintLine("Неверный тип категории.", Colors.ConsoleColorType.Red);
            }
        }

        static void CreateOperationCommandExample(IOperationFacade operationFacade, ICategoryFacade categoryFacade)
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

            // В данном примере создаем объект категории с введённым ID (на практике выбирают из списка)
            var category = new Category(categoryId, opType == OperationType.Income ? CategoryType.Income : CategoryType.Expense, "Пример категории");
            ICommand command = new CreateOperationCommand(operationFacade, opType, accountId, amount, category, description);
            ICommand timedCommand = new TimedCommandDecorator(command);
            timedCommand.Execute();
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

        static void ShowBankAccounts(IFinanceManager manager)
        {
            Colors.PrintLine("Список банковских счетов:", Colors.ConsoleColorType.Cyan);
            foreach (var account in manager.GetBankAccounts())
            {
                Console.WriteLine(account);
            }
        }

        static void ShowCategories(IFinanceManager manager)
        {
            Colors.PrintLine("Список категорий:", Colors.ConsoleColorType.Cyan);
            foreach (var category in manager.GetCategories())
            {
                Console.WriteLine(category);
            }
        }

        static void ShowOperations(IFinanceManager manager)
        {
            Colors.PrintLine("Список операций:", Colors.ConsoleColorType.Cyan);
            foreach (var operation in manager.GetOperations())
            {
                Console.WriteLine(operation);
            }
        }

        static void ExportDataToCsv(IFinanceManager manager)
        {
            Colors.Print("Введите директорию для экспорта CSV: ", Colors.ConsoleColorType.White);
            string dir = Console.ReadLine()?.Trim();
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
            string path = Console.ReadLine()?.Trim();
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
            string path = Console.ReadLine()?.Trim();
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
            string dir = Console.ReadLine()?.Trim();
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
            string path = Console.ReadLine()?.Trim();
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
            string path = Console.ReadLine()?.Trim();
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
    }
}
