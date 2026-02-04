using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductApi.Application.DTOS;
using ProductApi.Application.Serialization;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProduct productInterface) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
    {
        var products = await productInterface.GetAllAsync();
        var productDtos = products.ToDtos();
        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await productInterface.GetByIdAsync(id);
        if (product == null)
            return NotFound("Product not found");

        var productDto = product.ToDto();
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
    {
        var product = productDto.ToEntity();
        product.Id = 0; // Ensure Id is not set for creation
        var response = await productInterface.CreateAsync(product);
        if (!response.Flag)
            return BadRequest(response.Message);

        // Retrieve the created product
        
        var createdProduct = await productInterface.GetByIdAsync(product.Id);
        var createdDto = createdProduct.ToDto();
        return CreatedAtAction(nameof(GetProduct), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
    {
        if (id != productDto.Id)
            return BadRequest("Id mismatch");

        var product = productDto.ToEntity();
        var response = await productInterface.UpdateAsync(product);
        if (!response.Flag)
            return BadRequest(response.Message);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productInterface.GetByIdAsync(id);
        if (product == null)
            return NotFound("Product not found");

        var response = await productInterface.DeleteAsync(product);
        if (!response.Flag)
            return BadRequest(response.Message);

        return NoContent();
    }
}