using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class HotelAmenityDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter amenity name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter amenity schedule")]
        public string Schedule { get; set; }

        [Required(ErrorMessage = "Please enter amenity description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter amenity icon from font awesome")]
        public string IconStyle { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
