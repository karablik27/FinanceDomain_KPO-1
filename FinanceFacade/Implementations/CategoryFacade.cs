using System;
using System.Collections.Generic;
using System.Linq;
using FinanceFacade.Interfaces;
using FinanceLibrary;

namespace FinanceLibrary.Facades.Implementations
{
    public class CategoryFacade : ICategoryFacade
    {
        private readonly DomainFactory _factory;
        private readonly Dictionary<Guid, Category> _categories;

        public CategoryFacade(DomainFactory factory)
        {
            _factory = factory;
            _categories = new Dictionary<Guid, Category>();
        }

        public Category Create(CategoryType type, string name)
        {
            var category = _factory.CreateCategory(type, name);
            _categories[category.Id] = category;
            return category;
        }

        public void Update(Guid id, string newName)
        {
            if (!_categories.ContainsKey(id))
                throw new KeyNotFoundException("Категория не найдена.");

            _categories[id].UpdateName(newName);
        }

        public void Delete(Guid id)
        {
            if (!_categories.ContainsKey(id))
                throw new KeyNotFoundException("Категория не найдена.");

            _categories.Remove(id);
        }

        public Category GetById(Guid id)
        {
            if (!_categories.ContainsKey(id))
                throw new KeyNotFoundException("Категория не найдена.");

            return _categories[id];
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories.Values;
        }

        public IEnumerable<Category> GetByType(CategoryType type)
        {
            return _categories.Values.Where(c => c.Type == type);
        }
    }
}
