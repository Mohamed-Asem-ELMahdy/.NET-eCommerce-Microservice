using System.Linq.Expressions;
using eCommerce.SharedLibrary.Response;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using eCommerce.SharedLibrary.Exception;

namespace ProductApi.Infrastructure.Repositories;

/// Generic repository implementation for Product entity.
/// Implements CRUD operations defined in IProduct interface.

public class ProductRepository(AppDbContext _context) : IProduct
{
    public async Task<Response> CreateAsync(Product entity)
    {
        try
        {
            var product = _context.Products.Add(entity).Entity;
            await _context.SaveChangesAsync();
            return product.Id > 0 ? new Response(true, "Product created successfully")
                : new Response(false, "Product creation failed");
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<Response> UpdateAsync(Product entity)
    {
        try
        {
            var product = await _context.Products.FindAsync(entity.Id);
            if (product is null)
                return new Response(false, "Product not found");

            _context.Entry(product).State = EntityState.Detached;
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
            return new Response(true, "Product updated successfully");
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<Response> DeleteAsync(Product entity)
    {
        try
        {
            var product = await _context.Products.FindAsync(entity.Id);
            if (product is null)
                return new Response(false, "Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new Response(true, "Product deleted successfully");
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return Enumerable.Empty<Product>();
        }
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Products.FindAsync(id);
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return null!;
        }
    }

    public async Task<Product> GetByExpressionAsync(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);
            return null!;
        }
    }
}
