using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extension;
using AutoMapper;

namespace API.Helper
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
            
            CreateMap<AppUser,MemberDTO>()
            .ForMember(dest=>dest.PhotoUrl,op=>op.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.calculateAge()));

            CreateMap<Photo,PhotoDTO>();
            CreateMap<UpdateMemberDTO,AppUser>();
        }
    }
}