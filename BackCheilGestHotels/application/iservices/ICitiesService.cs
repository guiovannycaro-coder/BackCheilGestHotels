using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface ICitiesService
    {
        List<CityDto> GetCities();
        int InsCity(CityDto city);
        bool UpdCity(CityDto city);
        bool DropCityById(int dato);
        List<CityDto> GetCityById(int dato);
        List<CityDto> GetCityByName(string dato);
    }
}
