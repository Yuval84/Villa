using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Villa_Client.Service.IService
{
    public interface IHotelAmenityService
    {
        public Task<IEnumerable<HotelAmenityDto>> GetHotelAmenities();
    }
}
