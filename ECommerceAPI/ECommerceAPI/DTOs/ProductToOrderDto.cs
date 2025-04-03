namespace ECommerceAPI.DTOs;

public class ProductToOrderDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int OrderId { get; set; }
}
