using eCommerce.SharedLibrary.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces;
/// Product-specific repository interface that extends generic CRUD operations.

public interface IProduct : IGenericInterface<Product>
{
    
}
