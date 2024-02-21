using AutoMapper;
using Gym.Models;
using Gym.Models.DTOs;

namespace Gym.Services;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, CompanyDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<Image, ImageDto>();
        CreateMap<Subscription, SubscriptionDto>();
    }
}