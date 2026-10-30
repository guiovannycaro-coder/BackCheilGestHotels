using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IUsersRepository
    {
        List<UsersDto> GetUsuarios();

        String InsUsuarios(UsersDto user);

        String UpdUsuarios(UsersDto user);
        String DropUserById(int dato);

        List<UsersDto> GetUsuariosById(int dato);

        List<UsersDto> GetUsuariosByName(string dato);

        RolesDto GetRolUser(string perfil);

     
    }
}
