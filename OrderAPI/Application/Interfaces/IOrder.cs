namespace Application.Interfaces;
using eCommerce.SharedLibrary.Interfaces;
using Domain.Entities;
public interface IOrder: IGenericInterface<Order>
{
    Task<IEnumerable<Order>> GetOrders();
    
}