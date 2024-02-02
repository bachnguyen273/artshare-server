using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OrderDetails?> GetByOrderIdAsync(int orderId)
        {
            return await _dbContext.OrderDetails.Where(t => t.OrderId == orderId).FirstOrDefaultAsync();
        }
    }
}