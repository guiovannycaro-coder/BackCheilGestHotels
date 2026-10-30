using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.application.services;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.api.controllers
{
    [ApiController]
    [Route("api/gestHotel/Department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentsService _departmentService;

        public DepartmentController(IDepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/gestHotel/Department
        [HttpGet("ObtenerTodos")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DepartmentDto>>> GetDepartments()
        {
            try
            {
                List<DepartmentDto> departments = _departmentService.GetDepartments();

                return Ok(new ApisResponse<List<DepartmentDto>>(
                    200,
                    "Departments retrieved successfully.",
                    departments
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
                        "An unexpected error occurred while retrieving departments.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/Department/1
       
        [HttpGet("ObtenerPorId")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DepartmentDto>>> GetDepartmentById([FromQuery]int dato)
        {
            try
            {
                List<DepartmentDto> departments = _departmentService.GetDepartmentById(dato);

                if (departments == null || departments.Count == 0)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Department not found.",
                        null
                    ));
                }

                return Ok(new ApisResponse<List<DepartmentDto>>(
                    200,
                    "Department retrieved successfully.",
                    departments
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
                        "An unexpected error occurred while retrieving the department.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/Department/search/Cundinamarca
      
        [HttpGet("ObtenerPorName")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DepartmentDto>>> GetDepartmentByName([FromQuery]string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The department name is required.",
                        null
                    ));
                }

                List<DepartmentDto> departments =
                    _departmentService.GetDepartmentByName(dato);

                return Ok(new ApisResponse<List<DepartmentDto>>(
                    200,
                    "Department search completed successfully.",
                    departments
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
                        "An unexpected error occurred while searching departments.",
                        null
                    ));
            }
        }

        // GET: api/gestHotel/Department/country/1
        [HttpGet("DepartmentCountry")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<List<DepartmentDto>>> GetDepartmentsByCountry([FromQuery]int dato)
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

                // El servicio actual no expone un método específico para filtrar por país.
                // Se filtra la lista obtenida por GetDepartments().
                List<DepartmentDto> departments = _departmentService
                    .GetDepartments()
                    .Where(d => d.DepContryId == dato)
                    .ToList();

                return Ok(new ApisResponse<List<DepartmentDto>>(
                    200,
                    "Departments retrieved successfully.",
                    departments
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
                        "An unexpected error occurred while retrieving departments by country.",
                        null
                    ));
            }
        }

        // POST: api/gestHotel/Department
        [HttpPost("InsertarDepartment")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<int>> CreateDepartment(
            [FromBody] DepartmentDto department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Department data is required.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The department name is required.",
                        null
                    ));
                }

                if (department.DepContryId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid country ID is required.",
                        null
                    ));
                }

                int newDepartmentId = _departmentService.InsDepartment(department);

                return StatusCode(201, new ApisResponse<int>(
                    201,
                    "Department created successfully.",
                    newDepartmentId
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
                        "An unexpected error occurred while creating the department.",
                        null
                    ));
            }
        }

        // PUT: api/gestHotel/Department/1
    
        [HttpPut("ActualizarDepartment")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> UpdateDepartment(
            [FromBody] DepartmentDto department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "Department data is required.",
                        null
                    ));
                }

                if (department == null || department.DepId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The route ID must match the department ID in the request body.",
                        null
                    ));
                }

                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "The department name is required.",
                        null
                    ));
                }

                if (department.DepContryId <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid country ID is required.",
                        null
                    ));
                }

                bool updated = _departmentService.UpdDepartment(department);

                if (!updated)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Department not found or could not be updated.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "Department updated successfully.",
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
                        "An unexpected error occurred while updating the department.",
                        null
                    ));
            }
        }

        // DELETE: api/gestHotel/Department/1
      
        [HttpDelete("DeleteDepartment")]
        [Produces("application/json")]
        public ActionResult<ApisResponse<string>> DeleteDepartment([FromQuery]int dato)
        {
            try
            {
                if (dato <= 0)
                {
                    return BadRequest(new ApisResponse<string>(
                        400,
                        "A valid department ID is required.",
                        null
                    ));
                }

                bool deleted = _departmentService.DropDepartmentById(dato);

                if (!deleted)
                {
                    return NotFound(new ApisResponse<string>(
                        404,
                        "Department not found or could not be deleted.",
                        null
                    ));
                }

                return Ok(new ApisResponse<string>(
                    200,
                    "Department deleted successfully.",
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
                        "An unexpected error occurred while deleting the department.",
                        null
                    ));
            }
        }
    }
}
