using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models
{
    public class CreateHotelDto 
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Hotel name is too long!")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Hotel address is too long!")]
        public string Address { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDto : CreateHotelDto
    {
        public int Id { get; set; }
        public CountryDto Country { get; set; }
    }

}
