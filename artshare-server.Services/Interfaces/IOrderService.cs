﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateOrderAsync(Order order);

        Task<bool> DeleteOrderAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersByCusIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersByArtIdAsync(int id);
        Task<bool> CreateOrderWithOrderDetailsAsync(Order_OrderDetailsCreateDTO dto);
    }
}