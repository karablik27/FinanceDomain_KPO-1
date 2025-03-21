﻿using System;
using System.Collections.Generic;
using FinanceLibrary;

namespace FinanceFacade.Interfaces
{
    public interface ICategoryFacade
    {
        Category Create(CategoryType type, string name);
        void Update(Guid id, string newName);
        void Delete(Guid id);
        Category GetById(Guid id);
        IEnumerable<Category> GetAll();
        IEnumerable<Category> GetByType(CategoryType type);
    }
}
