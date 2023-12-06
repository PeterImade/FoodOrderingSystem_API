using AutoMapper;
using FoodOrderingSystem_API.DTOs.Customer;
using FoodOrderingSystem_API.DTOs.Menu;
using FoodOrderingSystem_API.DTOs.Order;
using FoodOrderingSystem_API.DTOs.OrderItem;
using FoodOrderingSystem_API.DTOs.RestaurantDTO;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CustomerCreateDTO, Customer>().ReverseMap();
            CreateMap<CustomerDTO, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDTO, Customer>().ReverseMap(); 
            
            CreateMap<MenuCreateDTO, Menu>().ReverseMap();
            CreateMap<MenuDTO, Menu>().ReverseMap();
            CreateMap<MenuUpdateDTO, Menu>().ReverseMap();
            
            CreateMap<OrderCreateDTO, Order>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderUpdateDTO, Order>().ReverseMap();
            
            CreateMap<OrderItemCreateDTO, OrderItem>().ReverseMap();
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
            CreateMap<OrderItemUpdateDTO, OrderItem>().ReverseMap();
            
            CreateMap<RestaurantCreateDTO, Restaurant>().ReverseMap();
            CreateMap<RestaurantDTO, Restaurant>().ReverseMap();
            CreateMap<RestaurantUpdateDTO, Restaurant>().ReverseMap();
        }

    }
}
