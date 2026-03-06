using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public record OrderDetailsDto( 
    [Required] int OrderId, 
    [Required] int ProductId, 
    [Required] int ClientId,
    [Required] string Name, 
    [Required, EmailAddress] string Email, 
    [Required] string Address,
    [Required] string TelephoneNumber, 
    [Required] string ProductName,
    [Required] int PurchaseQuantity, 
    [Required] decimal UnitPrice,
    [Required] decimal TotalPrice, [Required] DateTime Date
    );