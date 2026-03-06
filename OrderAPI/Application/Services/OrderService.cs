using System.Net.Http.Json;
using Application.Interfaces;
using Domain.DTOs;
using Polly.Registry;

namespace Application.Services;
using Polly;
public class OrderService(
    HttpClient httpClient,
    IOrder orderInterface,
    ResiliencePipelineProvider<string> resiliencePipeline
    ):IOrderService
{

    public async Task<ProductDTO?> GetProduct(int productId)
    {
        var getProduct = await httpClient.GetAsync($"api/products/{productId}");
        if (!getProduct.IsSuccessStatusCode)
        {
            return null!;
        }
        return await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
    }
    
    // get user 
    public async Task<AppUserDto> GetUser(int userId)
    {
        var getUser = await httpClient.GetAsync($"api/users/{userId}");
        if (!getUser.IsSuccessStatusCode)
            return null!;
        var product = await getUser.Content.ReadFromJsonAsync<AppUserDto>();
        return product!;
    }
    
    public async Task<IEnumerable<OrderDto>> GetOrdersByClient(int clientId)
    {
        var orders = await orderInterface.GetOrders();
        var clientOrders = orders.Where(o => o.ClientId == clientId);
        return clientOrders.Select(o => new OrderDto(
            o.Id,
            o.ProductId,
            o.ClientId,
            o.Quantity,
            o.Date));
    }

    public async Task<OrderDetailsDto?> GetOrderDetails(int orderId)
    {
        var order = await orderInterface.GetByIdAsync(orderId);
        if (order is null || order.Id <= 0)
            return null!;
        var retryPipline = resiliencePipeline.GetPipeline("my-pipline");
        var productDto = await retryPipline.ExecuteAsync(async token => await GetProduct(order.ProductId));
        var appUserDto = await retryPipline.ExecuteAsync(async token => await GetUser(order.ClientId));

        return new OrderDetailsDto(
            order.Id,
            productDto!.Id,
            appUserDto!.Id,
            appUserDto.Name,
            appUserDto.Email,
            appUserDto.Address,
            appUserDto.TelephoneNumber,
            productDto.Name,
            order.Quantity,
            productDto.Price,
            productDto.Price * order.Quantity,
            order.Date
        );
    }
}


public record ProductDTO(
    int Id,
    string Name,
    int Quantity,
    decimal Price
);

public record AppUserDto(
    int Id,
    string Name,
    string Email,
    string TelephoneNumber,
    string Password,
    string Role,
    string Address
    );