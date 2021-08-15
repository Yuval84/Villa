using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Data;
using Models;

namespace Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelRoomDto, HotelRoom>().ReverseMap();
            CreateMap<HotelRoomImageDto, HotelRoomImage>().ReverseMap();
            CreateMap<HotelAmenityDto, HotelAmenity>().ReverseMap();

            CreateMap<RoomOrderDetailsDto, RoomOrderDetails>();
            CreateMap<RoomOrderDetails, RoomOrderDetailsDto>().ForMember(x=>x.HotelRoomDto, opt=>opt.MapFrom(c=>c.HotelRoom));

        }
    }
}
