using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repo;

    public OrdersController(IOrderRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _repo.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    // ✅ Tworzy puste zamówienie (bez produktów)
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
        var order = await _repo.GetByIdAsync(id);
        if (order is null) return NotFound();

        await _repo.DeleteAsync(order);
        return NoContent();
    }
}
