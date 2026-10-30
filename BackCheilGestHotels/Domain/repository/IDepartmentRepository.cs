using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IDepartmentRepository
    {
        List<DepartmentDto> GetDepartmentsAsync();

        DepartmentDto GetDepartmentByIdAsync(int depId);

        List<DepartmentDto> GetDepartmentsByCountryIdAsync(int countId);

        String InsDepartmentAsync(DepartmentDto department);

        String UpdDepartmentAsync(DepartmentDto department);

        String DropDepartmentByIdAsync(int depId);
    }
}
