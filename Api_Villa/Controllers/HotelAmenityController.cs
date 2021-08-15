using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Repository.IRepository;
using Models;

namespace Api_Villa.Controllers
{
    [Route("api/[controller]")]
    public class HotelAmenityController : Controller
    {
        private readonly IAmenityRepository _amenityRepository;

        public HotelAmenityController(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAmenities()
        {
            var amenities = await _amenityRepository.GetAllHotelAmenity();
            return Ok(amenities);
        }




    }
}
