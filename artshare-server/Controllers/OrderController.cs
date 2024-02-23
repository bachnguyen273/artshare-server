using artshare_server.Contracts.DTOs;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
    }
}
