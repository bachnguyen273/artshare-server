using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        Task<List<OrderDetailDTO>?> GetByOrderIdAsync(int orderId);
        Task<List<OrderDetails>> GetOrderDetailsByOrdersAsync(List<Order> orders);
    }
}