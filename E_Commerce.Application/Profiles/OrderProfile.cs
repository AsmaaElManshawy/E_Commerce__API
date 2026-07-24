using AutoMapper;
using E_Commerce.Application.DTOs.IdentityDtos;
using E_Commerce.Application.DTOs.OrdersDtos;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace E_Commerce.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(x => x.DeliveryMethod , o => o.MapFrom(x => x.DeliveryMethod.ShortName))
                .ForMember(x => x.DeliveryMethodCost , o => o.MapFrom(x => x.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.ProductId))
                .ForMember(x => x.ProductName, o => o.MapFrom(x => x.Product.ProductName))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<OrderPictureUrlResolver>());

        }
    }
}
