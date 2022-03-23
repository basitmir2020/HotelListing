using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetAll(requestParams);
            var results = _mapper.Map<IList<CountryDTO>>(countries);
            return Ok(results);
        }
        [HttpGet("{id:int}",Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _unitOfWork
                    .Countries
                    .Get(q => q.Id == id, new List<string> { "Hotels" });
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(result);
        }
        [Authorize(Roles="Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO createCountryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Request attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
            var country = _mapper.Map<Country>(createCountryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
        }
        [Authorize(Roles="Administrator")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id,[FromBody] UpdateCountryDTO updateCountryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Request attempt in {nameof(UpdateCountryDTO)}");
                return BadRequest(ModelState);
            }
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateCountryDTO)}");
                return BadRequest("Submitting Data Is Invalid");
            }
            _mapper.Map(updateCountryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();

            return NoContent();
        }

        [Authorize(Roles = "Administration")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest("Submitting Data Is Invalid");
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
