using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailService;
        public OrderDetailController(IOrderDetailsService orderDetailService)
        {
            _orderDetailService = orderDetailService;
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
    }
}
