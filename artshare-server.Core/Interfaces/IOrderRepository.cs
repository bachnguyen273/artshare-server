using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<OrderDTO>> GetOrdersByCusIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersByArtIdAsync(int id);
        Task<List<Order>> GetOrdersByAccountIdAsync(int accountId);
    }
}