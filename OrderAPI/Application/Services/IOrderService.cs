using Domain.DTOs;

namespace Application.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersByClient(int clientId);
    Task<OrderDetailsDto?> GetOrderDetails(int orderId);
}