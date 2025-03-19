using System;
using FinanceFacade.Interfaces;
using FinanceLibrary;

public class CreateCategoryCommand : ICommand
{
    private readonly ICategoryFacade _categoryFacade;
    private readonly CategoryType _type;
    private readonly string _name;

    public CreateCategoryCommand(ICategoryFacade categoryFacade, CategoryType type, string name)
    {
        _categoryFacade = categoryFacade;
        _type = type;
        _name = name;
    }

    public void Execute()
    {
        var category = _categoryFacade.Create(_type, _name);
        Console.WriteLine($"Создана категория: {category}");
    }
}
