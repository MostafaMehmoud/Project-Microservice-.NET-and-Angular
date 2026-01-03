using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Commands;
using Ordering.Application.Respones;
using Ordering.Core.Entities;

namespace Ordering.Application.Mapper
{
    public class OrderMappingProfile:Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order,CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order,CheckoutOrderCommandV2>().ReverseMap();
            CreateMap<Order,UpdateOrderCommand>().ReverseMap();
            CreateMap<CheckoutOrderCommand,BasketCheckoutEvent>().ReverseMap();
            CreateMap<CheckoutOrderCommandV2, BasketCheckoutEventV2>().ReverseMap();

        }
    }
}
