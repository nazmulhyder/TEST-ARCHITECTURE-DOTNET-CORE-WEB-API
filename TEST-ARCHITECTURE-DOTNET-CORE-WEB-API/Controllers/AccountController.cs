using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private IMapper _mapper;

        public AccountController(UserManager<User> userManager,  ILogger<AccountController> logger, IMapper mapper)
        {
            //this._signInManager = signInManager;
            this._userManager = userManager;
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await this._userManager.CreateAsync(user,userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code,error.Description);
                    }

                    return BadRequest(ModelState);
                }

                await _userManager.AddToRolesAsync(user, userDto.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return StatusCode(500, "Internal Server Error!Pls try again later!");
            }
        }

        [HttpPost("Login")]
        public  Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
            //_logger.LogInformation($"Registration Attempt for {userLoginDto.Email}");
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //try
            //{
            //    var result =
            //        await _signInManager.PasswordSignInAsync(userLoginDto.Email, userLoginDto.Password, false, false);

            //    if (!result.Succeeded)
            //    {
            //        return Unauthorized(userLoginDto);

            //    }

            //    return Accepted();
            //}
            //catch (Exception ex)
            //{
            //    this._logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
            //    return StatusCode(500, "Internal Server Error!Pls try again later!");
            //}
        }
    }
}
