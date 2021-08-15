using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class AmenityRepository : IAmenityRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AmenityRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<HotelAmenityDto> CreateHotelAmenity(HotelAmenityDto hotelAmenity)
        {
            var amenity = _mapper.Map<HotelAmenityDto, HotelAmenity>(hotelAmenity);
            amenity.CreatedBy = "";
            amenity.CreatedDate = DateTime.UtcNow;
            var addHotelAmenity = await _db.HotelAmenities.AddAsync(amenity);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelAmenity, HotelAmenityDto>(addHotelAmenity.Entity);
        }

        public async Task<HotelAmenityDto> UpdateHotelAmenity(int amenityId, HotelAmenityDto hotelAmenity)
        {
            var amenityDetails = await _db.HotelAmenities.FindAsync(amenityId);
            var amenity = _mapper.Map<HotelAmenityDto, HotelAmenity>(hotelAmenity, amenityDetails);
            amenity.UpdateBy = "";
            amenity.UpdateDate = DateTime.UtcNow;
            var updateAmenity = _db.HotelAmenities.Update(amenity);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelAmenity, HotelAmenityDto>(updateAmenity.Entity);
        }

        public async Task<int> DeleteHotelAmenity(int amenityId, string userId)
        {
            var amenityDetails = await _db.HotelAmenities.FindAsync(amenityId);
            if (amenityDetails != null)
            {
                _db.HotelAmenities.Remove(amenityDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDto>> GetAllHotelAmenity()
        {
            return _mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDto>>(
                await _db.HotelAmenities.ToListAsync());
        }

        public async Task<HotelAmenityDto> GetHotelAmenity(int amenityId)
        {
            var amenity = await _db.HotelAmenities.FirstOrDefaultAsync(x => x.Id == amenityId);
            if (amenity == null)
                return null;

            return _mapper.Map<HotelAmenity, HotelAmenityDto>(amenity);
        }

        public async Task<HotelAmenityDto> IsSameNameAmenityAlreadyExist(string name)
        {
            try
            {
                var amenity =
                    await _db.HotelAmenities.FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
                return _mapper.Map<HotelAmenity, HotelAmenityDto>(amenity);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new HotelAmenityDto();
        }
    }
}
