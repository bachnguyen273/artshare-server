using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly IMapper _mapper;

        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public OrderRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<OrderDTO>> GetOrdersByArtIdAsync(int id)
        {
            var list= await _dbContext.OrderDetails.Where(x=>x.ArtworkId==id).Include(x=>x.Order).Select(x=>x.Order).DistinctBy(x=>x.OrderId).ToListAsync(); 
            return _mapper.Map<List<OrderDTO>>(list);
        }

        public async Task<List<OrderDTO>> GetOrdersByCusIdAsync(int id)
        {
            var list = await _dbContext.Orders.Where(x => x.CustomerId == id).ToListAsync();
            return _mapper.Map<List<OrderDTO>>(list);
        }
    }
}