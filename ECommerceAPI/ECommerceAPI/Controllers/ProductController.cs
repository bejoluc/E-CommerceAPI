﻿using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repo;

    public ProductController(IProductRepository repo)
    {
        _repo = repo;
    }

    // 1️⃣ Dodanie produktu do bazy (bez przypisania do zamówienia)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };

        await _repo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // 2️⃣ Dodanie produktu do zamówienia (czyli do "koszyka")
    [HttpPost("add-to-order")]
    public async Task<IActionResult> AddToOrder([FromBody] ProductToOrderDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            OrderId = dto.OrderId
        };

        await _repo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product updatedProduct)
    {
        if (id != updatedProduct.Id)
            return BadRequest();

        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = updatedProduct.Name;
        existing.Price = updatedProduct.Price;
        await _repo.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return NotFound();

        await _repo.DeleteAsync(product);
        return NoContent();
    }
}
