using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        Task<OrderDetails?> GetByOrderIdAsync(int orderId);
    }
}