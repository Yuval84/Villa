using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Business.Repository
{
    public class RoomOrderRepository : IRoomOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public RoomOrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<RoomOrderDetailsDto> Create(RoomOrderDetailsDto details)
        {
            try
            {
                details.CheckInDate = details.CheckInDate.Date;
                details.CheckOutDate = details.CheckOutDate.Date;
                var order = _mapper.Map<RoomOrderDetailsDto, RoomOrderDetails>(details);
                order.Status = SD.Status_Pending;
                var addedOrder = await _db.RoomOrderDetails.AddAsync(order);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(addedOrder.Entity);
            }
            catch
            {
                return null;
            }
        }



        public async Task<RoomOrderDetailsDto> MarkPaymentSuccessful(int id)
        {
            var data = await _db.RoomOrderDetails.FindAsync(id);
            if (data == null)
            {
                return null;
            }

            if (!data.IsPaymentSuccessful)
            {
                data.IsPaymentSuccessful = true;
                data.Status = SD.Status_Booked;
                var markPaymentSuccessful = _db.RoomOrderDetails.Update(data);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(markPaymentSuccessful.Entity);
            }

            return new RoomOrderDetailsDto();
        }



        public async Task<RoomOrderDetailsDto> GetRoomOrderDetails(int roomOrderId)
        {
            try
            {
                RoomOrderDetailsDto roomOrderDetails =
                    _mapper.Map<RoomOrderDetails, RoomOrderDetailsDto>(await _db.RoomOrderDetails.
                        Include(u => u.HotelRoom).ThenInclude(x => x.HotelRoomImages)
                        .FirstOrDefaultAsync(x => x.Id == roomOrderId));

                roomOrderDetails.HotelRoomDto.TotalDays =
                    roomOrderDetails.CheckOutDate.Subtract(roomOrderDetails.ActualCheckInDate).Days;
                return roomOrderDetails;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<IEnumerable<RoomOrderDetailsDto>> GetAllRoomOrderDetails()
        {
            try
            {
                IEnumerable<RoomOrderDetailsDto> roomOrders =
                    _mapper.Map<IEnumerable<RoomOrderDetails>, IEnumerable<RoomOrderDetailsDto>>(
                        _db.RoomOrderDetails.Include(u => u.HotelRoom));
                return roomOrders;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateOrderStatus(int roomOrderId, string status)
        {
            try
            {
                var roomOrder = await _db.RoomOrderDetails.FirstOrDefaultAsync(u => u.Id == roomOrderId);
                if (roomOrder == null)
                    return false;

                roomOrder.Status = status;
                if (status == SD.Status_CheckedIn)
                {
                    roomOrder.ActualCheckInDate = DateTime.Now;
                }

                if (roomOrder.Status == SD.Status_CheckedOut_Completed)
                {
                    roomOrder.ActualCheckOutDate = DateTime.Now;
                }

                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        
    }
}
