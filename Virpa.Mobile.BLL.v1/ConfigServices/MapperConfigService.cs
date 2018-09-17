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

            CreateMap<ApplicationUser, UserDetails>();

            CreateMap<AspNetUsers, UserDetails>();

            CreateMap<Skills, GetSkillsModel>();

            CreateMap<Files, GetFilesListResponse>();

            CreateMap<PostMyFeedModel, Feeds>()
                .ForMember(p => p.UpVoteCounts, f => f.ResolveUsing(p => 0))
                .ForMember(p => p.BiddingCounts, f => f.ResolveUsing(p => 0))
                .ForMember(p => p.CreatedAt, f => f.ResolveUsing(p => DateTime.UtcNow))
                .ForMember(p => p.UpdatedAt, f => f.ResolveUsing(p => DateTime.UtcNow))
                .ForMember(p => p.ExpiredAt, f => f.ResolveUsing(p => DateTime.UtcNow.AddDays(p.ExpiredOn)))
                .ForMember(p => p.Status, f => f.ResolveUsing(p => 0));

            CreateMap<Feeds, PostMyFeedDetailResponseModel>()
                .ForMember(p => p.FeedId, f => f.ResolveUsing(p => p.Id));

            CreateMap<Followers, PostMyFollowerResponseModel>();
        }
    }

    public static class MapperConfigService {
        public static IServiceCollection RegisterMapper(this IServiceCollection services) {

            services.AddAutoMapper();

            return services;
        }
    }

}