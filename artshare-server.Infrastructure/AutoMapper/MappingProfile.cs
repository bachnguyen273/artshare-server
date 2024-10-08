﻿using artshare_server.ApiModels.DTOs;
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
            // ACCOUNT
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, GetAccountDTO>().ReverseMap();
            CreateMap<Account, CreateAccountDTO>()
                .ForMember(src => src.Role, des => des.MapFrom(des => EnumMapper<AccountRole>.MapType(des.Role.ToString())))
                .ReverseMap();
            CreateMap<Account, UpdateAccountDTO>().ReverseMap();

            // ARTWORK
            CreateMap<Artwork, ArtworkDTO>().ReverseMap();
            CreateMap<Artwork, CreateArtworkDTO>()
                .ForMember(src => src.Status, des => des.MapFrom(des => EnumMapper<ArtworkStatus>.MapType(des.Status.ToString())))
                .ReverseMap();
            CreateMap<Artwork, UpdateArtworkDTO>().ReverseMap();
            CreateMap<Artwork, GetArtworkDTO>()
                .ForMember(x => x.Name, opt => opt.MapFrom(dest => dest.Genre.Name))
                .ForMember(src => src.ArtworkStatus, des => des.MapFrom(des => EnumMapper<ArtworkStatus>.MapType(des.Status.ToString())))
                .ReverseMap();            
            // GENRE
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Genre, GetGenreDTO>().ReverseMap();
            CreateMap<Genre, CreateGenreDTO>().ReverseMap();
            CreateMap<Genre, UpdateGenreDTO>().ReverseMap();

            // ORDER
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, GetOrderDTO>().ReverseMap();
            CreateMap<Order, CreateOrderDTO>().ReverseMap();
            CreateMap<Order, GetUserOrderDTO>().ForMember(x => x.FullName, opt => opt.MapFrom(dest => dest.Customer.FullName)).
                ForMember(x => x.Title, opt => opt.MapFrom(dest => dest.Artwork.Title)).ReverseMap();
         

            // REPORT
            CreateMap<Report, ReportDTO>()
                .ForMember(src => src.Category, des => des.MapFrom(des => EnumMapper<ReportCategory>.MapType(des.Category.ToString())))
                .ForMember(src => src.Status, des => des.MapFrom(des => EnumMapper<ReportStatus>.MapType(des.Status.ToString())))
                .ReverseMap();
            CreateMap<Report, CreateReportDTO>().ReverseMap();
            CreateMap<Report, UpdateReportDTO>().ReverseMap();
            CreateMap<Report, GetReportDTO>().ReverseMap();

            //COMMENT
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, GetCommentDTO>().ReverseMap();
            CreateMap<Comment, CreateCommentDTO>().ReverseMap();
            CreateMap<Comment, UpdateCommentDTO>().ReverseMap();

            // Order_OrderDetails
        }
    }
}