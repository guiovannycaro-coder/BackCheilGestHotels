using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;

namespace BackCheilGestHotels.application.services
{
    public class UsersService : IUsersService
    {

        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public List<UsersDto> GetUsuarios()
        {
            try
            {
                List<UsersDto> users = _usersRepository.GetUsuarios();

                if (users == null || users.Count == 0)
                {
                    return new List<UsersDto>();
                }

                return users;

            } catch (Exception e)
            {
                throw new BackCheilGestHotelException(  500,"Error al consultar los usuarios.", e);
            }
        }


        public string UpdUsuarios(UsersDto user)
        {
            try
            {
                return _usersRepository.UpdUsuarios(user);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating user.",
                    ex
                );
            }
        }

        public string DropUserById(int dato)
        {
            try
            {
                return _usersRepository.DropUserById(dato);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting user.",
                    ex
                );
            }
        }

        public RolesDto GetPerfilUsuario(string perfil)
        {
            try
            {
                return _usersRepository.GetRolUser(perfil);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving roles assigned to user.",
                    ex
                );
            }
        }



        public List<UsersDto> GetUsuariosById(int dato)
        {
            try
            {
                return _usersRepository.GetUsuariosById(dato);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving user by ID.",
                    ex
                );
            }
        }

        public List<UsersDto> GetUsuariosByName(string dato)
        {
            try
            {
                return _usersRepository.GetUsuariosByName(dato);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving users by name.",
                    ex
                );
            }
        }


        public string InsUsuarios(UsersDto user)
        {
            Console.WriteLine(">>> ENTRO A UsersService.InsUsuarios");
            try
            {
                Console.WriteLine(">>> ENTRO A UsersService.InsUsuarios");
                return _usersRepository.InsUsuarios(user);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error inserting user.",
                    ex
                );
            }
        }


        public string upUsuarios(UsersDto user)
        {
            try
            {
                return _usersRepository.UpdUsuarios(user);
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating user.",
                    ex
                );
            }
        }
    }
}
