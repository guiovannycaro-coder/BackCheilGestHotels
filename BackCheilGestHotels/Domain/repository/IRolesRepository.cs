using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IRolesRepository
    {
        List<RolesDto> GetRolesAsync();

        int InsRolesAsync(RolesDto role);

        bool UpdRolesAsync(RolesDto role);

        bool DropRoleByIdAsync(int rolId);

        RolesDto GetRoleByIdAsync(int rolId);

        List<RolesDto> GetRolesByNameAsync(string name);

    }
}
