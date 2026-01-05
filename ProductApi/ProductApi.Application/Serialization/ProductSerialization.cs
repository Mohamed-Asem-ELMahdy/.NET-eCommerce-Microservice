using ProductApi.Application.DTOS;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Serialization;

public static class ProductSerialization
{
    public static ProductDTO ToDto(this Product product)
    {
        return new ProductDTO(
            product.Id,
            product.Name,
            product.Quantity,
            product.Price
        );
    }

    public static Product ToEntity(this ProductDTO productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Quantity = productDto.Quantity,
            Price = productDto.Price
        };
    }
}
