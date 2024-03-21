using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<GetOrderDTO>> GetOrdersByCusIdAsync(int id);
        Task<List<GetOrderDTO>> GetOrdersByArtIdAsync(int id);
        Task<List<GetOrderDTO>> GetOrdersByAccountIdAsync(int accountId);
        Task<List<GetOrderDTO>> GetOrders();
    }
}