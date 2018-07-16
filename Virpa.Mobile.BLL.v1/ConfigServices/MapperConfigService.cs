using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Virpa.Mobile.DAL.v1.Entities.Mobile;
using Virpa.Mobile.DAL.v1.Identity;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.BLL.v1.ConfigServices {

    public class MapperProfile : Profile {
        
        public MapperProfile() {

            CreateMap<CreateUserModel, ApplicationUser>()
                .ForMember(p => p.Email, f => f.ResolveUsing(p => p.Email))
                .ForMember(p => p.CreatedAt, f => f.ResolveUsing(p => DateTime.UtcNow))
                .ForMember(p => p.UpdatedAt, f => f.ResolveUsing(p => DateTime.UtcNow));

            CreateMap<ApplicationUser, UserResponse>();

            CreateMap<AspNetUsers, UserResponse>();
        }
    }

    public static class MapperConfigService {
        public static IServiceCollection RegisterMapper(this IServiceCollection services) {

            services.AddAutoMapper();

            return services;
        }
    }

}