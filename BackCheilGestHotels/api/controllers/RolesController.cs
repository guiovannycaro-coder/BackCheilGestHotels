using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/Roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        // GET: api/gestHotel/Roles
        // Consultar todos los roles
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public IActionResult GetRoles()
        {
            try
            {
                List<RolesDto> roles = _rolesService.GetRoles();

                return Ok(new ApisResponse<List<RolesDto>>(
                    0,
                    "Roles retrieved successfully.",
                    roles
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        ex.Message,
                        null
                    )
                );
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while retrieving roles.",
                    null
                ));
            }
        }

        // Buscar roles por nombre

        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public IActionResult GetRolesByName([FromQuery] string name)
        {
            try
            {
                List<RolesDto> roles = _rolesService.GetRolesByName(name);

                return Ok(new ApisResponse<List<RolesDto>>(
                    0,
                    "Roles search completed successfully.",
                    roles
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new ApisResponse<string>(
                   500,
                    ex.Message,
                    null
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while searching roles.",
                    null
                ));
            }
        }
        // GET: api/gestHotel/Roles/5
        // Consultar un rol por ID

        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public IActionResult GetRoleById([FromQuery] int rolId)
        {
            try
            {
                List<RolesDto> roles = _rolesService.GetRolesById(rolId);

                if (roles == null || roles.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Role not found.",
                        null
                    ));
                }

                return Ok(new ApisResponse<List<RolesDto>>(
                    0,
                    "Role retrieved successfully.",
                    roles
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    ex.Message,
                    null
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while retrieving the role.",
                    null
                ));
            }
        }

        // GET: api/gestHotel/Roles/search/Admin
        // Buscar roles por nombre
   
        

        // POST: api/gestHotel/Roles
        // Crear un rol
      
        [HttpPost("InsertarRol")]
        [Produces("application/json")]
        public IActionResult CreateRole([FromBody] RolesDto role)
        {
            try
            {
                if (role == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Role data is required.",
                        null
                    ));
                }

                int rolId = _rolesService.InsRol(role);

                return CreatedAtAction(
                    nameof(GetRoleById),
                    new { rolId },
                    new ApisResponse<int>(
                        0,
                        "Role created successfully.",
                        rolId
                    )
                );
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    ex.Message,
                    null
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while creating the role.",
                    null
                ));
            }
        }

        // PUT: api/gestHotel/Roles/5
        // Actualizar un rol
     

        [HttpPut("ActualizarRol")]
        [Produces("application/json")]
        public IActionResult UpdateRole( [FromBody] RolesDto role)
        {
            try
            {
                if (role == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Role data is required.",
                        null
                    ));
                }

                if (role == null || role.RolId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The route ID does not match the role ID in the request body.",
                        null
                    ));
                }

                bool updated = _rolesService.UpdRol(role);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Role not found or no changes were made.",
                        null
                    ));
                }

                return Ok(new ApisResponse<bool>(
                    0,
                    "Role updated successfully.",
                    true
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    ex.Message,
                    null
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while updating the role.",
                    null
                ));
            }
        }

        // DELETE: api/gestHotel/Roles/5
        // Eliminar un rol
      
        [HttpDelete("DeleteRol")]
        [Produces("application/json")]
        public IActionResult DeleteRole([FromQuery] int rolId)
        {
            try
            {
                bool deleted = _rolesService.DropRolById(rolId);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Role not found.",
                        null
                    ));
                }

                return Ok(new ApisResponse<bool>(
                    0,
                    "Role deleted successfully.",
                    true
                ));
            }
            catch (BackCheilGestHotelException ex)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    ex.Message,
                    null
                ));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApisResponse<string>(
                    500,
                    "An unexpected error occurred while deleting the role.",
                    null
                ));
            }
        }
    }
}
