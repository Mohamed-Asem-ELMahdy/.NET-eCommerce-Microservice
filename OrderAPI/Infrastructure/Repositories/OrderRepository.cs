using System.Linq.Expressions;
using Application.Interfaces;
using Domain.Entities;
using eCommerce.SharedLibrary.Exception;
using eCommerce.SharedLibrary.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrder
{
    public async Task<Response> CreateAsync(Order entity)
    {
        try
        {
            var order = context.Orders.Add(entity).Entity;
            await context.SaveChangesAsync();
            return order.Id > 0
                ? new Response(true, "Order placed successfully")
                : new Response(false, "Failed to place order");
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<Response> UpdateAsync(Order entity)
    {
        try
        {
            var order = await context.Orders.FindAsync(entity.Id);
            if (order is null)
                return new Response(false, "Order not found");

            context.Entry(order).State = EntityState.Detached;
            context.Orders.Update(entity);
            await context.SaveChangesAsync();
            return new Response(true, "Order updated successfully");
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<Response> DeleteAsync(Order entity)
    {
        try
        {
            var order = await context.Orders.FindAsync(entity.Id);
            if (order is null)
                return new Response(false, "Order not found");

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return new Response(true, "Order deleted successfully");
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return new Response(false, ex.Message);
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        try
        {
            return await context.Orders.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return Enumerable.Empty<Order>();
        }
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        try
        {
            return await context.Orders.FindAsync(id) ?? null!;
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return null!;
        }
    }

    public async Task<Order> GetByExpressionAsync(Expression<Func<Order, bool>> predicate)
    {
        try
        {
            return await context.Orders.AsNoTracking().FirstOrDefaultAsync(predicate) ?? null!;
        }
        catch (Exception ex)
        {
            LogException.Log(ex);
            return null!;
        }
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await GetAllAsync();
    }
}
