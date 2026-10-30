using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/Security")]
    public class UsersServiceController : ControllerBase
    {

        private readonly IUsersSeguridadService _usersService;

        public UsersServiceController(IUsersSeguridadService usersService)
        {
            _usersService = usersService;
        }


        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginDto datos)
        {
            try
            {
                bool authenticated = _usersService.Authenticate(datos);

                if (!authenticated)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid email or password."
                    });
                }

                return Ok(new
                {
                    message = "Authentication successful.",
                    authenticated = true
                });
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost("RememberUser")]
        public IActionResult RememberUser([FromBody] LoginDto datos)
        {
            try
            {
                string result = _usersService.RememberUser(datos);

                if (result == "User not found or inactive.")
                {
                    return NotFound(new
                    {
                        message = result
                    });
                }

                return Ok(new
                {
                    message = result
                });
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("GetProfile")]
        public IActionResult GetProfile([FromQuery] string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return BadRequest(new
                    {
                        message = "Email is required."
                    });
                }

                string profile = _usersService.getRole(dato);

                if (profile == "User not found or inactive.")
                {
                    return NotFound(new
                    {
                        message = profile
                    });
                }

                return Ok(new
                {
                    message = "Profile retrieved successfully.",
                    profile = profile
                });
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
