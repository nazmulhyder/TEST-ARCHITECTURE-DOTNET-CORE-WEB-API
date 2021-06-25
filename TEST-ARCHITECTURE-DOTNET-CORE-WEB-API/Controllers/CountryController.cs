using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await this._unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDto>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex,$"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error!Pls try again later!");
            } 
        }

        [Authorize]
        [HttpGet("GetCountry/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await this._unitOfWork.Countries.Get(c=>c.Id == id,new List<string>{ "Hotels" });
                var result= _mapper.Map<CountryDto>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error!Pls try again later!");
            }
        }
    }
}
