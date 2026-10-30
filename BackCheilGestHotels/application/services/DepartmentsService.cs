using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.repositories;

namespace BackCheilGestHotels.application.services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsService(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // DELETE: Eliminar departamento por ID
        public bool DropDepartmentById(int dato)
        {
            try
            {
                string result =
                    _departmentRepository.DropDepartmentByIdAsync(dato);

                return result == "Department deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting department.",
                    ex);
            }
        }

        // GET: Obtener departamento por ID
        public List<DepartmentDto> GetDepartmentById(int dato)
        {
            try
            {
                DepartmentDto department =
                    _departmentRepository.GetDepartmentByIdAsync(dato);

                if (department == null)
                {
                    return new List<DepartmentDto>();
                }

                return new List<DepartmentDto> { department };
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving department by ID.",
                    ex);
            }
        }

        // GET: Buscar departamentos por nombre
        public List<DepartmentDto> GetDepartmentByName(string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return new List<DepartmentDto>();
                }

                // El repositorio actual no tiene búsqueda por nombre.
                // Se filtra la lista obtenida desde la base de datos.
                return _departmentRepository
                    .GetDepartmentsAsync()
                    .Where(d => d.Name.Contains(
                        dato,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error searching departments by name.",
                    ex);
            }
        }

        // GET: Obtener todos los departamentos
        public List<DepartmentDto> GetDepartments()
        {
            try
            {
                return _departmentRepository.GetDepartmentsAsync();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving departments.",
                    ex);
            }
        }

        // POST: Crear departamento
        public int InsDepartment(DepartmentDto department)
        {
            try
            {
                string result =
                    _departmentRepository.InsDepartmentAsync(department);

                const string prefix =
                    "Department created successfully. ID: ";

                if (result.StartsWith(prefix) &&
                    int.TryParse(
                        result.Substring(prefix.Length),
                        out int newId))
                {
                    return newId;
                }

                return 0;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error creating department.",
                    ex);
            }
        }

        // PUT: Actualizar departamento
        public bool UpdDepartment(DepartmentDto department)
        {
            try
            {
                string result =
                    _departmentRepository.UpdDepartmentAsync(department);

                return result == "Department updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating department.",
                    ex);
            }
        }
    }
}
