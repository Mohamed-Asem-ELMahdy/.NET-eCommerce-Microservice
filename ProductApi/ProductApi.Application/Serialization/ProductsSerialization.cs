using ProductApi.Application.DTOS;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Serialization;

public static class ProductsSerialization
{
    public static IEnumerable<ProductDTO> ToDtos(this IEnumerable<Product> products)
    {
        return products.Select(product => product.ToDto());
    }

    public static IEnumerable<Product> ToEntities(this IEnumerable<ProductDTO> productDtos)
    {
        return productDtos.Select(dto => dto.ToEntity());
    }
}
