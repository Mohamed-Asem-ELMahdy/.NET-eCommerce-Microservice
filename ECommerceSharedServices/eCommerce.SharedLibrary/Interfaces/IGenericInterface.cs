using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eCommerce.SharedLibrary.Response;

namespace eCommerce.SharedLibrary.Interfaces;
/// A generic interface for CRUD operations.

public interface IGenericInterface<T> where T : class
{
    /// Creates a new entity asynchronously.
    Task<Response.Response> CreateAsync(T entity);
    
    /// Updates an existing entity asynchronously.
    Task<Response.Response> UpdateAsync(T entity);
    
    /// Deletes an entity asynchronously.
    Task<Response.Response> DeleteAsync(T entity);

    /// Retrieves all entities asynchronously.
    Task<IEnumerable<T>> GetAllAsync();
    
    /// Retrieves an entity by its identifier asynchronously.
    Task<T> GetByIdAsync(int id);
    
    /// Retrieves an entity based on a specified expression asynchronously.
    Task<T> GetByExpressionAsync(Expression<Func<T, bool>> predicate);
}