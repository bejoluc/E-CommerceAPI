using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;

    public ProductController(IProductRepository productRepo, IOrderRepository orderRepo)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
            return BadRequest("Invalid product data.");

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        await _productRepo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPost("add-to-order")]
    public async Task<IActionResult> AddToOrder([FromBody] AssignProductToOrderDto dto)
    {
        if (dto.ProductId <= 0 || dto.OrderId <= 0)
            return BadRequest("Invalid IDs.");

        var product = await _productRepo.GetByIdAsync(dto.ProductId);
        if (product is null) return NotFound($"Product with ID {dto.ProductId} not found.");

        var order = await _orderRepo.GetByIdAsync(dto.OrderId);
        if (order is null) return NotFound($"Order with ID {dto.OrderId} not found.");

        product.OrderId = dto.OrderId;
        await _productRepo.UpdateAsync(product);

        return Ok($"Product {dto.ProductId} assigned to order {dto.OrderId}.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _productRepo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID.");

        var product = await _productRepo.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto dto)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
            return BadRequest("Invalid data.");

        var existing = await _productRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = dto.Name;
        existing.Price = dto.Price;
        await _productRepo.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID.");

        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return NotFound();

        await _productRepo.DeleteAsync(product);
        return NoContent();
    }
}
