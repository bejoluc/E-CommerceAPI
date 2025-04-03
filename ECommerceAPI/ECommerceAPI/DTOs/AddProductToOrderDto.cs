namespace ECommerceAPI.DTOs;

public class AddProductToOrderDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
