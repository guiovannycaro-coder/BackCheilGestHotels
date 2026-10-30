using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface IDepartmentsService
    {
        List<DepartmentDto> GetDepartments();
        int InsDepartment(DepartmentDto department);
        bool UpdDepartment(DepartmentDto department);
        bool DropDepartmentById(int dato);
        List<DepartmentDto> GetDepartmentById(int dato);
        List<DepartmentDto> GetDepartmentByName(string dato);
    }
}
