using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{

    [ApiController]
    [Route("api/gestHotel/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<UsersDto>>> getAll()
        {
            try {
                string descriptionError = "SUCCESSFULL";

                List<UsersDto> infoDto = _usersService.GetUsuarios();
                var response = new ApisResponse<List<UsersDto>>(
       StatusCodes.Status200OK,
       descriptionError,
       infoDto
   );
                return Ok(response);


            }
            catch (Exception e)
            {
                var response = new ApisResponse<List<UsersDto>>(
          StatusCodes.Status500InternalServerError,
          "INTERNAL SERVER ERROR -> THE INFORMATION WAS NOT FOUND: "
              + e.Message,
          null
      );

                return StatusCode(
           StatusCodes.Status500InternalServerError,
           response
       );

            }

        }



       
        // BUSCAR USUARIOS POR NOMBRE
        [HttpGet("BuscarPorNombre")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<UsersDto>>> GetByName(
            [FromQuery] string dato)
        {
            try
            {
                List<UsersDto> infoDto =
                    _usersService.GetUsuariosByName(dato);

                var response = new ApisResponse<List<UsersDto>>(
                    StatusCodes.Status200OK,
                    "SUCCESSFUL",
                    infoDto
                );

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<List<UsersDto>>(
                        500,
                        "INTERNAL SERVER ERROR: Error searching users by name.",
                        null
                    )
                );
            }
        }

        // INSERTAR USUARIO
        [HttpPost("InsertarUsuarios")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> Insert(
            [FromBody] UsersDto user)
        {
            try
            {
                string result = _usersService.InsUsuarios(user);

                var response = new ApisResponse<string>(
                    StatusCodes.Status201Created,
                    "USER CREATED SUCCESSFULLY.",
                    result
                );

                return StatusCode(201, response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "INTERNAL SERVER ERROR: Error inserting user.",
                        null
                    )
                );
            }
        }

        // ACTUALIZAR USUARIO
        [HttpPut("actualizarUsuarios")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> Update(
            [FromBody] UsersDto user)
        {
            try
            {
                string result = _usersService.UpdUsuarios(user);

                if (result.Contains("was not found",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(
                        new ApisResponse<string>(
                            404,
                            result,
                            null
                        )
                    );
                }

                var response = new ApisResponse<string>(
                    StatusCodes.Status200OK,
                    "USER UPDATED SUCCESSFULLY.",
                    result
                );

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "INTERNAL SERVER ERROR: Error updating user.",
                        null
                    )
                );
            }
        }

        // ELIMINAR USUARIO POR ID
        [HttpDelete("eliminarUsuarios")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> Delete([FromQuery]  int dato)
        {
            try
            {
                string result = _usersService.DropUserById(dato);

                if (result.Contains("was not found",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(
                        new ApisResponse<string>(
                            404,
                            result,
                            null
                        )
                    );
                }

                var response = new ApisResponse<string>(
                    StatusCodes.Status200OK,
                    "USER DELETED SUCCESSFULLY.",
                    result
                );

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<string>(
                        500,
                        "INTERNAL SERVER ERROR: Error deleting user.",
                        null
                    )
                );
            }
        }


        // CONSULTAR USUARIO POR ID
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<UsersDto>>> GetById([FromQuery] int dato)
        {
            try
            {
                List<UsersDto> infoDto = _usersService.GetUsuariosById(dato);

                if (infoDto.Count == 0)
                {
                    return NotFound(
                        new ApisResponse<List<UsersDto>>(
                            StatusCodes.Status404NotFound,
                            "USER NOT FOUND.",
                            null
                        )
                    );
                }

                var response = new ApisResponse<List<UsersDto>>(
                    StatusCodes.Status200OK,
                    "SUCCESSFUL",
                    infoDto
                );

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<List<UsersDto>>(
                        500,
                        "INTERNAL SERVER ERROR: Error retrieving user by ID.",
                        null
                    )
                );
            }
        }


        // CONSULTAR ROLES ASIGNADOS A UN USUARIO
        [HttpGet("ObtenerUserRole")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<RolesDto>> GetUserRoles(
          [FromQuery] string dato)
        {
            try
            {
                RolesDto infoDto = _usersService.GetPerfilUsuario(dato);

                var response = new ApisResponse<RolesDto>(
                    StatusCodes.Status200OK,
                    "SUCCESSFUL",
                    infoDto
                );

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new ApisResponse<RolesDto>(
                        500,
                        "INTERNAL SERVER ERROR: Error retrieving user role.",
                        null
                    )
                );
            }
        }
    }
}


