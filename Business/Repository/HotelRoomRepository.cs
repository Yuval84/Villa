using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelRoomRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }



        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoomDto)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto);
            hotelRoom.CreatedBy = "";
            hotelRoom.CreatedDate = DateTime.UtcNow;

            var addedRoom = await _db.HotelRooms.AddAsync(hotelRoom);

            await _db.SaveChangesAsync();

            return _mapper.Map<HotelRoom, HotelRoomDto>(addedRoom.Entity);
        }

        public async Task<HotelRoomDto> UpdateHotelRoom(int roomId, HotelRoomDto hotelRoomDto)
        {
            try
            {
                if (roomId == hotelRoomDto.Id)
                {
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto, roomDetails);
                    room.UpdateBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDto>(updatedRoom.Entity);
                }
                else
                {
                    //Invalid roomId
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;

            }
        }

        public async Task<HotelRoomDto> GetHotelRoom(int roomId, string checkInDate, string checkOutDate)
        {
            try
            {
                HotelRoomDto hotelRoom =
                    _mapper.Map<HotelRoom, HotelRoomDto>(await _db.HotelRooms
                        .Include(x => x.HotelRoomImages) //include the images based on the foreign key (HotelRoomImages)
                        .FirstOrDefaultAsync(x => x.Id == roomId));

                if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    hotelRoom.IsBooked = await IsRoomBooked(roomId, checkInDate, checkOutDate);
                }

                return hotelRoom;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDate, string checkOutDate)
        {
            try
            {
                IEnumerable<HotelRoomDto> hotelRoomDtos =
                      _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDto>>(_db.HotelRooms
                          .Include(x => x.HotelRoomImages));
                if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    foreach (HotelRoomDto hotelRoom in hotelRoomDtos)
                    {
                        hotelRoom.IsBooked = await IsRoomBooked(hotelRoom.Id, checkInDate, checkOutDate);
                    }
                }

                return hotelRoomDtos;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<HotelRoomDto> IsRoomUniq(string name, int roomId = 0)
        {
            try
            {
                if (roomId == 0)
                {
                    HotelRoomDto hotelRoom =
                        _mapper.Map<HotelRoom, HotelRoomDto>(
                            await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name == name));
                    return hotelRoom;
                }
                else
                {
                    HotelRoomDto hotelRoom =
                        _mapper.Map<HotelRoom, HotelRoomDto>(
                            await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name == name && x.Id != roomId));
                    return hotelRoom;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<bool> IsRoomBooked(int roomId, string checkInDatestr, string checkOutDatestr)
        {
            try
            {
                if (!string.IsNullOrEmpty(checkInDatestr) && !string.IsNullOrEmpty(checkOutDatestr))
                {
                    DateTime checkInDate = DateTime.ParseExact(checkInDatestr, "MM/dd/yyyy", null);
                    DateTime checkOutDate = DateTime.ParseExact(checkOutDatestr, "MM/dd/yyyy", null);
                    var existingBooking = await _db.RoomOrderDetails
                        .Where(x => x.RoomId == roomId && x.IsPaymentSuccessful &&
                                    ((checkInDate < x.CheckOutDate &&
                                     checkInDate.Date >= x.CheckInDate)
                                     || (checkOutDate.Date > x.CheckInDate.Date &&
                                     checkInDate.Date <= x.CheckInDate.Date)))
                        .FirstOrDefaultAsync();

                    if (existingBooking != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> DeleteRoom(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);
            if (roomDetails != null)
            {
                var allImages = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
                //foreach (var image in allImages)
                //{
                //    if (File.Exists(image.RoomImageUrl))
                //    {
                //        File.Delete(image.RoomImageUrl);
                //    }
                //}
                _db.HotelRoomImages.RemoveRange(allImages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }

            return 0;
        }
    }
}
