using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/City")]
    public class CityController : ControllerBase
    {
        private readonly ICitiesService _cityService;

        public CityController(ICitiesService cityService)
        {
            _cityService = cityService;
        }

        // GET: api/gestHotel/City
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CityDto>>> GetCities()
        {
            try
            {
                List<CityDto> cities = _cityService.GetCities();

                return Ok(new ApisResponse<List<CityDto>>(
                    200,
                    "Cities retrieved successfully.",
                    cities
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
                        "An unexpected error occurred while retrieving cities.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/City/5
      
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CityDto>>> GetCityById([FromQuery]int dato)
        {
            try
            {
                List<CityDto> cities = _cityService.GetCityById(dato);

                if (cities == null || cities.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "City not found.",
                        null
                    ));
                }

                return Ok(new ApisResponse<List<CityDto>>(
                    200,
                    "City retrieved successfully.",
                    cities
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
                        "An unexpected error occurred while retrieving the city.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/City/search/Bogota
       
        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<CityDto>>> GetCityByName([FromQuery]string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The city name is required.",
                        null
                    ));
                }

                List<CityDto> cities = _cityService.GetCityByName(dato);

                return Ok(new ApisResponse<List<CityDto>>(
                    200,
                    "City search completed successfully.",
                    cities
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
                        "An unexpected error occurred while searching cities.",
                        null
                    ));
            }
        }

        // POST: api/gestHotel/City
     
        [HttpPost("InsertarCity")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<int>> CreateCity([FromBody] CityDto city)
        {
            try
            {
                if (city == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "City data is required.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(city.CityName))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The city name is required.",
                        null
                    ));
                }

                if (city.CityidDepartament <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid department ID is required.",
                        null
                    ));
                }

                int newCityId = _cityService.InsCity(city);

                return StatusCode(201, new ApisResponse<int>(
                    201,
                    "City created successfully.",
                    newCityId
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
                        "An unexpected error occurred while creating the city.",
                        null
                    ));
            }
        }

        // PUT: api/gestHotel/City/5
   
        [HttpPut("ActualizarCity")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> UpdateCity(
            
            [FromBody] CityDto city)
        {
            try
            {
                if (city == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "City data is required.",
                        null
                    ));
                }

                if (city == null || city.CityId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The route ID must match the city ID in the request body.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(city.CityName))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The city name is required.",
                        null
                    ));
                }

                if (city.CityidDepartament <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid department ID is required.",
                        null
                    ));
                }

                bool updated = _cityService.UpdCity(city);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "City not found or could not be updated.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "City updated successfully.",
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
                        "An unexpected error occurred while updating the city.",
                        null
                    ));
            }
        }

        // DELETE: api/gestHotel/City/5
 
        [HttpDelete("DeleteCity")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> DeleteCity([FromQuery]int dato)
        {
            try
            {
                if (dato <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid city ID is required.",
                        null
                    ));
                }

                bool deleted = _cityService.DropCityById(dato);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "City not found or could not be deleted.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "City deleted successfully.",
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
                        "An unexpected error occurred while deleting the city.",
                        null
                    ));
            }
        }
    }
}
