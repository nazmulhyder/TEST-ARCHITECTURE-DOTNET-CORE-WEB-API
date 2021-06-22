using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Service
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserLoginDto userLoginDto);
        Task<string> CreateToken();
    }
}
