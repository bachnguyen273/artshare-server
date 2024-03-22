using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        public async Task<List<GetOrderDTO>> GetOrdersByArtIdAsync(int id)
        {
            var list = await _dbContext.Orders.Where(x => x.ArtworkId == id).ToListAsync();
            
            return _mapper.Map<List<GetOrderDTO>>(list);
           
        }

        public async Task<List<GetOrderDTO>> GetOrdersByCusIdAsync(int id)
        {
            var list = await _dbContext.Orders.Where(x => x.CustomerId == id).ToListAsync();
            return _mapper.Map<List<GetOrderDTO>>(list);
        }

        // OrderRepository.cs
        public async Task<List<GetOrderDTO>> GetOrdersByAccountIdAsync(int accountId)
        {
            List<Order> list = await _dbContext.Orders.Where(o => o.CustomerId == accountId).ToListAsync();
            return _mapper.Map<List<GetOrderDTO>>(list);
        }

        public async Task<List<GetOrderDTO>> GetOrders()
        {
            List<Order> list = await _dbContext.Orders.ToListAsync();
            return _mapper.Map<List<GetOrderDTO>>(list);
        }      
    }
}