using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface ICityRepository
    {
        List<CityDto> GetCities();

        CityDto GetCityById(int cityId);

        List<CityDto> GetCitiesByName(string name);

        List<CityDto> GetCitiesByDepartmentId(int depId);

        String InsCity(CityDto city);

        String UpdCity(CityDto city);

        String DropCityById(int cityId);
    }
}
