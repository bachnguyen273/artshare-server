using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Utils;
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

        public async Task<PagedResult<GetOrderDTO>> GetAllOrdersAsync<T>(OrderFilters filters)
        {
            // Apply filtering
            var items = await _unitOfWork.OrderRepo.GetOrders();
            IQueryable<GetOrderDTO> filteredItemsQuery = items.AsQueryable();

            if (filters.OrderId != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.OrderId == filters.OrderId);
            if (filters.CustomerId != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.CustomerId == filters.CustomerId);
            if (filters.OrderDate != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.OrderDate == filters.OrderDate);
            if (filters.TotalPrice != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.TotalPrice == filters.TotalPrice);

            // Apply sorting
            if (!string.IsNullOrEmpty(filters.SortBy))
            {
                switch (filters.SortBy)
                {
                    case "orderId":
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => item.OrderId) :
                            filteredItemsQuery.OrderByDescending(item => item.OrderId);
                        break;
                    case "customerId":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.CustomerId) :
                             filteredItemsQuery.OrderByDescending(item => item.CustomerId);
                        break;
                    case "orderDate":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.OrderDate) :
                             filteredItemsQuery.OrderByDescending(item => item.OrderDate);
                        break;
                    case "totalPrice":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.TotalPrice) :
                             filteredItemsQuery.OrderByDescending(item => item.TotalPrice);
                        break;
                    default:
                        // Handle other sorting filter using Utils.GetPropertyValue
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => Helpers.GetPropertyValue(item, filters.SortBy)) :
                            filteredItemsQuery.OrderByDescending(item => Helpers.GetPropertyValue(item, filters.SortBy));
                        break;
                }
            }

            // Apply paging
            var pagedItems = filteredItemsQuery
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList(); // Materialize the query

            return new PagedResult<GetOrderDTO>
            {
                Items = pagedItems,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = pagedItems.Count()
            };
        }

        public async Task<GetOrderDTO?> GetOrderByIdAsync(int orderId)
        {
            if (orderId > 0)
            {
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId);
                return _mapper.Map<GetOrderDTO>(order);
            }
            return null;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            await _unitOfWork.OrderRepo.AddAsync(order);
            _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetOrderDTO>> GetOrdersByCusIdAsync(int id)
        {
            var orderList = await _unitOfWork.OrderRepo.GetOrdersByCusIdAsync(id);
            return orderList;
        }

        public async Task<List<GetOrderDTO>> GetOrdersByArtIdAsync(int id)
        {
            var orderList = await _unitOfWork.OrderRepo.GetOrdersByArtIdAsync(id);
            return orderList;
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
    }
}