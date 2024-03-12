﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Contracts.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Models;
using AutoMapper;

namespace artshare_server.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
       public MappingProfile()
        {
            CreateMap<RegisterDTO, Account>()
                .ForMember(des => des.Role, src => src.MapFrom(src => EnumMapper<AccountRole>.MapType(src.Role)));
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDetails, OrderDetailDTO>();
            CreateMap<Watermark, WatermarkDTO>();
            CreateMap<WatermarkCreateDTO, Watermark>();

            CreateMap<CreateOrderDTO, Order>();
            
        }
    }
}