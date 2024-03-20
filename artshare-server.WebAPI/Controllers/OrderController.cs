using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artshare_server.ApiModels.DTOs;
using AutoMapper;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;

namespace artshare_server.WebAPI.Controllers;

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
    public async Task<IActionResult> GetOrders([FromQuery] OrderFilters orderFilters)
    {

        var result = await _orderService.GetAllOrdersAsync<Order>(orderFilters);
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
    [HttpGet]
    public async Task<IActionResult> GetAllOrdersOfCreator([FromQuery] int id)
    {

        var result = await _orderService.GetAllOrdersOfCreator(id);
        return Ok(new SucceededResponseModel()
        {
            Status = Ok().StatusCode,
            Message = "Success",
            Data = result
        });


    }
}
