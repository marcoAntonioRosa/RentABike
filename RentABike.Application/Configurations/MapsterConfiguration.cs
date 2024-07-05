using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;

namespace RentABike.Application.Configurations;

public static class MapsterConfiguration
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<UserForRegistrationDto, User>
            .NewConfig()
            .Map(dest => dest.UserName, src => src.Email);

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly()); 
    }
}