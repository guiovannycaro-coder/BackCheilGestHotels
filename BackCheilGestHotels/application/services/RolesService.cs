using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.repositories;

namespace BackCheilGestHotels.application.services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        // Eliminar un rol por ID
        public bool DropRolById(int dato)
        {
            try
            {
                return _rolesRepository.DropRoleByIdAsync(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while deleting the role.",
                    ex);
            }
        }

        // Consultar todos los roles
        public List<RolesDto> GetRoles()
        {
            try
            {
                return _rolesRepository.GetRolesAsync();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving roles.",
                    ex);
            }
        }

        // Consultar un rol por ID
        public List<RolesDto> GetRolesById(int dato)
        {
            try
            {
                RolesDto role = _rolesRepository.GetRoleByIdAsync(dato);

                if (role == null)
                {
                    return new List<RolesDto>();
                }

                return new List<RolesDto> { role };
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving the role by ID.",
                    ex);
            }
        }

        // Buscar roles por nombre
        public List<RolesDto> GetRolesByName(string dato)
        {
            try
            {
                return _rolesRepository.GetRolesByNameAsync(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while searching roles by name.",
                    ex);
            }
        }

        // Insertar un rol
        public int InsRol(RolesDto rol)
        {
            try
            {
                return _rolesRepository.InsRolesAsync(rol);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while inserting the role.",
                    ex);
            }
        }

        // Actualizar un rol
        public bool UpdRol(RolesDto rol)
        {
            try
            {
                return _rolesRepository.UpdRolesAsync(rol);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while updating the role.",
                    ex);
            }
        }
    }
}
