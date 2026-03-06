using Domain.Entities;
namespace Domain.DTOs.OrderConversions;

public class OrderConversion
{
    public static (OrderDto?, IEnumerable<OrderDto>?) FromEntity(Order? order, IEnumerable<Order>? orders)
    {
        // Logic to handle a single order conversion
        if (order != null && orders == null)
        {
            var singleOrder = new OrderDto(
                order.Id, 
                order.ProductId, 
                order.ClientId, 
                order.Quantity, 
                order.Date);
            return (singleOrder, null);
        }

        // Logic to handle a list of orders conversion
        if (orders != null && order == null)
        {
            var orderList = orders.Select(o => new OrderDto(
                o.Id, 
                o.ProductId, 
                o.ClientId, 
                o.Quantity, 
                o.Date));
            return (null, orderList);
        }

        return (null, null);
    }
}