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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetHotels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await this._unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"something went wrong to the {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

        [HttpGet("GetHotel/{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await this._unitOfWork.Hotels.Get(c=>c.Id == id,new List<string>{ "Country" });
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateHotel")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"Invalid post attempt in {nameof(CreateHotel)}");
                    return BadRequest(ModelState);
                }

                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute(nameof(GetHotel), new {id = hotel.Id},hotel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

        [Authorize]
        [HttpPut("UpdateHotel/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id,[FromBody] UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                this._logger.LogError($"Invalid update attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = await this._unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    this._logger.LogError($"Invalid update attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(hotelDto, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return Ok(new
                {
                    success = true,
                    message = "Updated Successfully!"
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

        [Authorize]
        [HttpDelete("DeleteHotel/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                this._logger.LogError($"Invalid delete attempt in {nameof(UpdateHotel)}");
                return BadRequest("Invalid delete attempt!");
            }

            try
            {
                var hotel = await this._unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    this._logger.LogError($"Invalid delete attempt in the {nameof(DeleteHotel)}");
                    return BadRequest("Does not exist with this id!");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();
                return Ok(new
                {
                    success = true,
                    message = "Deleted Successfully!"
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal server error! try again later!");
            }
        }

    }
}
