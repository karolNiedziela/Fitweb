﻿using Backend.Core.Entities;
using Backend.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int id);

        Task<Product> GetAsync(string name);

        Task<PagedList<Product>> GetAllAsync(string name, string category, PaginationQuery paginationQuery);

        Task AddAsync(Product product);

        Task AddRangeAsync(List<Product> products);

        Task DeleteAsync(Product product);

        Task UpdateAsync(Product product);

        Task<bool> AnyAsync(Expression<Func<Product, bool>> expression);
    }
}
