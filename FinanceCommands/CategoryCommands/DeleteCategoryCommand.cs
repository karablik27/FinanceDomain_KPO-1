using System;
using FinanceFacade.Interfaces;

public class DeleteCategoryCommand : ICommand
{
    private readonly ICategoryFacade _categoryFacade;
    private readonly Guid _categoryId;

    public DeleteCategoryCommand(ICategoryFacade categoryFacade, Guid categoryId)
    {
        _categoryFacade = categoryFacade;
        _categoryId = categoryId;
    }

    public void Execute()
    {
        _categoryFacade.Delete(_categoryId);
        Console.WriteLine($"Категория {_categoryId} удалена.");
    }
}
