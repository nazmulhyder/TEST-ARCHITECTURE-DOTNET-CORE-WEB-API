using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
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
        [HttpGet("GetCountry/{id:int}",Name = "GetCountry")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateCountry")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Invalid post attempt in {nameof(CreateCountry)}");
                    return BadRequest(ModelState);
                }

                var country = _mapper.Map<Country>(countryDto);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute(nameof(GetCountry), new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

        [Authorize]
        [HttpPut("UpdateCountry/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto countryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                this._logger.LogError($"Invalid update attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var country = await this._unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    this._logger.LogError($"Invalid update attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Data Not Found!");
                }

                _mapper.Map(countryDto, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return Ok(new
                {
                    success = true,
                    message = "Updated Successfully!"
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal server error!");
            }
        }

        [Authorize]
        [HttpDelete("DeleteCountry/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                this._logger.LogError($"Invalid delete attempt in {nameof(DeleteCountry)}");
                return BadRequest("Invalid delete attempt!");
            }

            try
            {
                var hotel = await this._unitOfWork.Countries.Get(q => q.Id == id);
                if (hotel == null)
                {
                    this._logger.LogError($"Invalid delete attempt in the {nameof(DeleteCountry)}");
                    return BadRequest("Does not exist with this id!");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                return Ok(new
                {
                    success = true,
                    message = "Deleted Successfully!"
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }
    }
}
