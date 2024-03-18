using artshare_server.Contracts.DTOs;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artshare_server.ApiModels.DTOs;
using AutoMapper;
using artshare_server.Core.Models;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersByCusId([FromQuery] int customerId)
        {

            var result = await _orderService.GetOrdersByCusIdAsync(customerId);
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });


        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {

            var result = await _orderService.GetAllOrdersAsync();
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });


        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersByArtId([FromQuery] int artId)
        {

            var result = await _orderService.GetOrdersByArtIdAsync(artId);
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });


        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO order)
        {
            var ord = _mapper.Map<Order>(order);
            var result = await _orderService.CreateOrderAsync(ord);
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderWithOrderDetails(Order_OrderDetailsCreateDTO dto)
        {
            var result = await _orderService.CreateOrderWithOrderDetailsAsync(dto);
            if (result)
            {
                return Ok(new SucceededResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success"
                });
            }
            else
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Failed"
                });
            }
        }
    }
}
