namespace ECommerce.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
