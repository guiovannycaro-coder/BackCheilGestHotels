using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface IRolesService
    {
        List<RolesDto> GetRoles();
        int InsRol(RolesDto rol);
        bool UpdRol(RolesDto rol);
        bool DropRolById(int dato);
        List<RolesDto> GetRolesById(int dato);
        List<RolesDto> GetRolesByName(string dato);
    }
}
