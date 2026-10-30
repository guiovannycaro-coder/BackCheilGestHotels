using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/Country")]
    public class CountryController : ControllerBase
    {
        private readonly ICountriesService _countryService;

        public CountryController(ICountriesService countryService)
        {
            _countryService = countryService;
        }

        // GET: api/gestHotel/Country
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CountryDto>>> GetCountries()
        {
            try
            {
                List<CountryDto> countries = _countryService.GetCountries();

                return Ok(new ApisResponse<List<CountryDto>>(
                    200,
                    "Countries retrieved successfully.",
                    countries
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while retrieving countries.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/Country/1
      
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CountryDto>>> GetCountryById([FromQuery]int dato)
        {
            try
            {
                List<CountryDto> countries = _countryService.GetCountryById(dato);

                if (countries == null || countries.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Country not found.",
                        null
                    ));
                }

                return Ok(new ApisResponse<List<CountryDto>>(
                    200,
                    "Country retrieved successfully.",
                    countries
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while retrieving the country.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/Country/search/Colombia

        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CountryDto>>> GetCountryByName([FromQuery]string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The country name is required.",
                        null
                    ));
                }

                List<CountryDto> countries = _countryService.GetCountryByName(dato);

                return Ok(new ApisResponse<List<CountryDto>>(
                    200,
                    "Country search completed successfully.",
                    countries
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while searching countries.",
                        null
                    ));
            }
        }

        // POST: api/gestHotel/Country
        [HttpPost("InsertarCountry")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<int>> CreateCountry(
            [FromBody] CountryDto country)
        {
            try
            {
                if (country == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Country data is required.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(country.CountName))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The country name is required.",
                        null
                    ));
                }

                int newCountryId = _countryService.InsCountry(country);

                return StatusCode(201, new ApisResponse<int>(
                    201,
                    "Country created successfully.",
                    newCountryId
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while creating the country.",
                        null
                    ));
            }
        }

        // PUT: api/gestHotel/Country/1
      
        [HttpPut("ActualizarCountry")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> UpdateCountry(
            
            [FromBody] CountryDto country)
        {
            try
            {
                if (country == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Country data is required.",
                        null
                    ));
                }

                if (country == null || country.CountId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The route ID must match the country ID in the request body.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(country.CountName))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The country name is required.",
                        null
                    ));
                }

                bool updated = _countryService.UpdCountry(country);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Country not found or could not be updated.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "Country updated successfully.",
                    null
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while updating the country.",
                        null
                    ));
            }
        }

        // DELETE: api/gestHotel/Country/1

        [HttpDelete("DeleteCountry")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> DeleteCountry([FromQuery]int dato)
        {
            try
            {
                if (dato <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid country ID is required.",
                        null
                    ));
                }

                bool deleted = _countryService.DropCountryById(dato);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Country not found or could not be deleted.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "Country deleted successfully.",
                    null
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500,
                        "An unexpected error occurred while deleting the country.",
                        null
                    ));
            }
        }
    }
}
