
using ECommerce.Domain.Entities;
namespace ECommerce.Application.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task DeleteAsync(Order order);
}
