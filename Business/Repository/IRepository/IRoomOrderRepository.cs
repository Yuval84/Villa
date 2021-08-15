using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IRoomOrderRepository
    {
        public Task<RoomOrderDetailsDto> Create(RoomOrderDetailsDto details);

        public Task<RoomOrderDetailsDto> MarkPaymentSuccessful(int id);

        public Task<RoomOrderDetailsDto> GetRoomOrderDetails(int roomOrderId);

        public Task<IEnumerable<RoomOrderDetailsDto>> GetAllRoomOrderDetails();

        public Task<bool> UpdateOrderStatus(int roomId, string status);

    }
}
