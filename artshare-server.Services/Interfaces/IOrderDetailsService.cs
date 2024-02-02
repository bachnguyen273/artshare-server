﻿using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        Task<IEnumerable<OrderDetails>> GetAllOrderDetailsAsync();

        Task<OrderDetails?> GetOrderDetailsByOrderIdAsync(int orderId);

        Task<bool> CreateOrderDetailsAsync(OrderDetails orderDetails);

        Task<bool> UpdateOrderDetailsAsync(OrderDetails orderDetails);

        Task<bool> DeleteOrderDetailsByOrderIdAsync(int orderId);
    }
}