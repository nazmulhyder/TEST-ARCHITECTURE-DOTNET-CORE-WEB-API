using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        [HttpGet("GetCountries")]
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
    }
}
