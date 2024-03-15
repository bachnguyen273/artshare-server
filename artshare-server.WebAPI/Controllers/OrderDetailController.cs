using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.Services;
using AutoMapper;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailService;
        private readonly IMapper _mapper;
        public OrderDetailController(IOrderDetailsService orderDetailService,IMapper mapper)
        {
            _orderDetailService = orderDetailService;
            _mapper = mapper;   
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderDetailsByOrderId([FromQuery] int orderId)
        {

            var result = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDetailDTO orderDetail)
        {
           var ordDetail= _mapper.Map<OrderDetails>(orderDetail);

            var result = await _orderDetailService.CreateOrderDetailsAsync(ordDetail);
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });
        }
    }
}
