using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Villa_Client.Service.IService
{
    public interface IRoomOrderDetailsService
    {
        public Task<RoomOrderDetailsDto> SaveRoomOrderDetails(RoomOrderDetailsDto details);

        public Task<RoomOrderDetailsDto> MarkPaymentSuccessfully(RoomOrderDetailsDto details);
    }
}
