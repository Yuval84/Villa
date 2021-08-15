using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using Villa_Client.Service.IService;


namespace Villa_Client.Service
{
    public class HotelRoomService : IHotelRoomService
    {
        private readonly HttpClient _client;

        public HotelRoomService(HttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<HotelRoomDto>> GetHotelRooms(string checkInDate, string checkOutDate)
        {
            var response = await _client.GetAsync($"api/hotelRoom?checkInDate={checkInDate}&checkOutDate={checkOutDate}");
            var content = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<IEnumerable<HotelRoomDto>>(content);
            return rooms;
        }

        public async Task<HotelRoomDto> GetHotelRoomDetails(int roomId, string checkInDate, string checkOutDate)
        {
            var response = await _client.GetAsync($"api/hotelRoom/{roomId}?checkInDate={checkInDate}&checkOutDate={checkOutDate}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var room = JsonConvert.DeserializeObject<HotelRoomDto>(content);
                return room;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }
    }
}
