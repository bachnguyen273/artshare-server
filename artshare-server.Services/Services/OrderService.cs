using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

namespace artshare_server.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orderList = await _unitOfWork.OrderRepo.GetAllAsync();
            return orderList;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            if (orderId > 0)
            {
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId);
                return order;
            }
            return null;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>> GetOrdersByCusIdAsync(int id)
        {
            var orderList = await _unitOfWork.OrderRepo.GetOrdersByCusIdAsync(id);
            return orderList;
        }

        public async Task<List<OrderDTO>> GetOrdersByArtIdAsync(int id)
        {
            var orderList = await _unitOfWork.OrderRepo.GetOrdersByArtIdAsync(id);
            return orderList;
        }
    }
}