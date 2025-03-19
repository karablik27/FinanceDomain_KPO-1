using System;
using FinanceFacade.Interfaces;

public class UpdateCategoryCommand : ICommand
{
    private readonly ICategoryFacade _categoryFacade;
    private readonly Guid _categoryId;
    private readonly string _newName;

    public UpdateCategoryCommand(ICategoryFacade categoryFacade, Guid categoryId, string newName)
    {
        _categoryFacade = categoryFacade;
        _categoryId = categoryId;
        _newName = newName;
    }

    public void Execute()
    {
        _categoryFacade.Update(_categoryId, _newName);
        Console.WriteLine($"Категория {_categoryId} обновлена новым именем: {_newName}");
    }
}
