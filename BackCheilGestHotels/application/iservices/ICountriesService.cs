using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface ICountriesService
    {
        List<CountryDto> GetCountries();
        int InsCountry(CountryDto country);
        bool UpdCountry(CountryDto country);
        bool DropCountryById(int dato);
        List<CountryDto> GetCountryById(int dato);
        List<CountryDto> GetCountryByName(string dato);
    }
}
