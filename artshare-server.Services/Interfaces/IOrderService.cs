﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;

namespace artshare_server.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<GetOrderDTO>> GetAllOrdersAsync<T>(OrderFilters orderFilters);

        Task<GetOrderDTO?> GetOrderByIdAsync(int orderId);

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateOrderAsync(Order order);

        Task<bool> DeleteOrderAsync(int orderId);
        Task<List<GetOrderDTO>> GetOrdersByCusIdAsync(int id);
        Task<List<GetOrderDTO>> GetOrdersByArtIdAsync(int id);
    }
}