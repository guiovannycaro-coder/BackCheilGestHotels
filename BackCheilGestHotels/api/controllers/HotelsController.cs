using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/Hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsService _hotelsService;

        public HotelsController(IHotelsService hotelsService)
        {
            _hotelsService = hotelsService;
        }

        // GET: api/gestHotel/Hotels
        // Consultar todos los hoteles
       
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public IActionResult GetHotels()
        {
            try
            {
                List<HotelDto> hotels = _hotelsService.GetHotels();

                return Ok(new ApisResponse<List<HotelDto>>(
                    0,
                    "Hotels retrieved successfully.",
                    hotels
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while retrieving hotels.", null));
            }
        }

        // GET: api/gestHotel/Hotels/5
        // Consultar hotel por ID
     
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public IActionResult GetHotelById([FromQuery] int dato)
        {
            try
            {
                List<HotelDto> hotels = _hotelsService.GetHotelById(dato);

                if (hotels == null || hotels.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404, "Hotel not found.", null));
                }

                return Ok(new ApisResponse<List<HotelDto>>(
                    0, "Hotel retrieved successfully.", hotels));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while retrieving the hotel.", null));
            }
        }

        // GET: api/gestHotel/Hotels/search/Marriott
        // Buscar hoteles por nombre
        
        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public IActionResult GetHotelByName([FromQuery] string name)
        {
            try
            {
                List<HotelDto> hotels = _hotelsService.GetHotelByName(name);

                return Ok(new ApisResponse<List<HotelDto>>(
                    0, "Hotel search completed successfully.", hotels));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while searching hotels.", null));
            }
        }

        // GET: api/gestHotel/Hotels/city/3
        // Consultar hoteles por ciudad
        // Este endpoint requiere agregar GetHotelsByCityId al servicio.
        // Se puede habilitar cuando exista ese método en IHotelsService.

        // POST: api/gestHotel/Hotels
        // Crear hotel
        [HttpPost("InsertarHotels")]
        [Produces("application/json")]
        public IActionResult CreateHotel([FromBody] HotelDto hotel)
        {
            try
            {
                if (hotel == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400, "Hotel data is required.", null));
                }

                if (string.IsNullOrWhiteSpace(hotel.HotName) ||
                    string.IsNullOrWhiteSpace(hotel.HotDescription))
                {
                    return BadRequest(new ApisResponse<string>(
                        400, "Hotel name and description are required.", null));
                }

                int hotelId = _hotelsService.InsHotel(hotel);

                return CreatedAtAction(
                    nameof(GetHotelById),
                    new { hotId = hotelId },
                    new ApisResponse<int>(
                        0, "Hotel created successfully.", hotelId)
                );
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while creating the hotel.", null));
            }
        }

        // PUT: api/gestHotel/Hotels/5
        // Actualizar hotel

        [HttpPut("ActualizarHotels")]
        [Produces("application/json")]
        public IActionResult UpdateHotel(int hotId, [FromBody] HotelDto hotel)
        {
            try
            {
                if (hotel == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400, "Hotel data is required.", null));
                }

                if (hotId != hotel.HotId)
                {
                    return BadRequest(new ApisResponse<string>(
                        400, "The route ID does not match the hotel ID in the request body.", null));
                }

                if (string.IsNullOrWhiteSpace(hotel.HotName) ||
                    string.IsNullOrWhiteSpace(hotel.HotDescription))
                {
                    return BadRequest(new ApisResponse<string>(
                        400, "Hotel name and description are required.", null));
                }

                bool updated = _hotelsService.UpdHotel(hotel);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404, "Hotel not found or no changes were made.", null));
                }

                return Ok(new ApisResponse<bool>(
                    0, "Hotel updated successfully.", true));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while updating the hotel.", null));
            }
        }

        // DELETE: api/gestHotel/Hotels/5
        // Eliminar hotel
    
        [HttpDelete("DeleteHotels")]
        [Produces("application/json")]
        public IActionResult DeleteHotel([FromQuery] int dato)
        {
            try
            {
                bool deleted = _hotelsService.DropHotelById(dato);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404, "Hotel not found.", null));
                }

                return Ok(new ApisResponse<bool>(
                    0, "Hotel deleted successfully.", true));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, ex.Message, null));
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new ApisResponse<string>(
                        500, "An unexpected error occurred while deleting the hotel.", null));
            }
        }

        [HttpGet("BuscarHotels")]
        [Produces("application/json")]
        public IActionResult BuscarHotels(
    [FromQuery] string? hotName,
    [FromQuery] int? hotRating)
        {
            try
            {


                List<HotelDto> hotels = _hotelsService.BuscarHotels(hotName, hotRating);

                return Ok(new ApisResponse<List<HotelDto>>(
                    0,
                    "Hotels retrieved successfully.",
                    hotels
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
                        "An unexpected error occurred while searching hotels.",
                        null));
            }
        }
    }
}
