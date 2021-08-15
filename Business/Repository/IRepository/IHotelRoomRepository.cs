using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoomDto);

        public Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto hotelRoomDto);

        public Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDate = null, string checkOutDate = null);

        public Task<int> DeleteRoom(int roomId);

        public Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null);

        public Task<HotelRoomDto> IsRoomUniq(string name, int roomId = 0);

        public Task<bool> IsRoomBooked(int roomId, string checkIn, string checkOut);
    }
}
