using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly IMapper _mapper;

        public OrderDetailsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public OrderDetailsRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<OrderDetailDTO>?> GetByOrderIdAsync(int orderId)
        {
            var list = await _dbContext.OrderDetails.Where(t => t.OrderId == orderId).ToListAsync();
            return _mapper.Map<List<OrderDetailDTO>>(list);
        }
    }
}