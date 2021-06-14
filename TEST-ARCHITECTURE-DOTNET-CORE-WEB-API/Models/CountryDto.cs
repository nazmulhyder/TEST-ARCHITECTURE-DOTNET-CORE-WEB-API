using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models
{
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long!")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 5, ErrorMessage = "Country short name is too long!")]
        public string ShortName { get; set; }
    }

    public class CountryDto : CreateCountryDto
    {
        public int Id { get; set; }
        public ICollection<HotelDto> Hotels { get; set; }

    }
}
