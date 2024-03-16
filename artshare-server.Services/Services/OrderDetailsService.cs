//using artshare_server.ApiModels.DTOs;
//using artshare_server.Core.Interfaces;
//using artshare_server.Core.Models;
//using artshare_server.Services.Interfaces;

//namespace artshare_server.Services.Services
//{
//    public class OrderDetailsService : IOrderDetailsService
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public OrderDetailsService(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<IEnumerable<OrderDetails>> GetAllOrderDetailsAsync()
//        {
//            var orderList = await _unitOfWork.OrderDetailsRepo.GetAllAsync();
//            return orderList;
//        }

//        public async Task<List<OrderDetailDTO>?> GetOrderDetailsByOrderIdAsync(int orderId)
//        {
//            if (orderId > 0)
//            {
//                var orderDetails = await _unitOfWork.OrderDetailsRepo.GetByOrderIdAsync(orderId);
//                _unitOfWork.SaveAsync();
//                return orderDetails;
//            }
//            return null;
//        }

//        public async Task<bool> CreateOrderDetailsAsync(OrderDetails orderDetails)
//        {
//            _unitOfWork.OrderDetailsRepo.AddAsync(orderDetails);
//            return true;
//        }

//        public async Task<bool> UpdateOrderDetailsAsync(OrderDetails orderDetails)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<bool> DeleteOrderDetailsByOrderIdAsync(int orderId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}