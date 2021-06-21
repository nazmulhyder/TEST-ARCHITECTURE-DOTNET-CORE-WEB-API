using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models
{
    public class UserLoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Password minimum length 8 is Required!")]
        public string Password { get; set; }
    }

    public class UserDto : UserLoginDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
