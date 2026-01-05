using System.Linq.Expressions;
using eCommerce.SharedLibrary.Response;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Repositories;

/// Generic repository implementation for Product entity.
/// Implements CRUD operations defined in IProduct interface.

public class ProductRepository : IProduct
{
    public Task<Response> CreateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Response> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Response> DeleteAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByExpressionAsync(Expression<Func<Product, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}
