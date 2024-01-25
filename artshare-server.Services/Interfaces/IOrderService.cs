﻿using artshare_server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order> GetOrderByIdAsync(int orderId);

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateOrderAsync(Order order);

        Task<bool> DeleteOrderAsync(int orderId);
    }
}
