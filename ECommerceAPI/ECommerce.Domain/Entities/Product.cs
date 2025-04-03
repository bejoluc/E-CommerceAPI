using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    [JsonIgnore]
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
