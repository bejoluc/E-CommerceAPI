using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerceAPI.DTOs;
using ECommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repo;
    private readonly IProductRepository _productRepo;
    private readonly AppDbContext _context;

    public OrdersController(
        IOrderRepository repo,
        IProductRepository productRepo,
        AppDbContext context)
    {
        _repo = repo;
        _productRepo = productRepo;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID.");

        var order = await _repo.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var order = new Order();
        await _repo.AddAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0) return BadRequest("Invalid ID.");

        var order = await _repo.GetByIdAsync(id);
        if (order is null) return NotFound();

        await _repo.DeleteAsync(order);
        return NoContent();
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToOrder([FromBody] AddProductToOrderDto dto)
    {
        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero.");

        var product = await _productRepo.GetByIdAsync(dto.ProductId);
        if (product == null) return NotFound("Product not found.");
        if (product.Stock < dto.Quantity) return BadRequest("Not enough stock available.");

        var order = await _repo.GetByIdAsync(dto.OrderId);
        if (order == null) return NotFound("Order not found.");

        // Odlicz dostępny stock
        product.Stock -= dto.Quantity;
        await _productRepo.UpdateAsync(product);

        // Tworzymy powiązanie
        var orderProduct = new OrderProduct
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        _context.OrderProducts.Add(orderProduct);
        await _context.SaveChangesAsync();

        return Ok($"Product {dto.ProductId} added to order {dto.OrderId} (qty: {dto.Quantity})");
    }
}
