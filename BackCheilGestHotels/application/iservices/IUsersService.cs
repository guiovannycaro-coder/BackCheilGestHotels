using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface IUsersService
    {
        List<UsersDto> GetUsuarios();
        string InsUsuarios(UsersDto user);
        string UpdUsuarios(UsersDto user);
        string DropUserById(int dato);
        List<UsersDto> GetUsuariosById(int dato);
        List<UsersDto> GetUsuariosByName(string dato);
        RolesDto GetPerfilUsuario(string perfil);
    }
}
