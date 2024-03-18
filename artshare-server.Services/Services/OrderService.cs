using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            await _unitOfWork.OrderRepo.AddAsync(order);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> CreateOrderWithOrderDetailsAsync(Order_OrderDetailsCreateDTO dto)
        {
            try
            {
                var createOrder = _mapper.Map<Order>(dto);
                await _unitOfWork.OrderRepo.AddAsync(createOrder);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
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