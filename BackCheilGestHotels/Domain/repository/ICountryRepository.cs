using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface ICountryRepository
    {
        List<CountryDto> GetCountries();


        String InsCountry(CountryDto country);

        String UpdCountry(CountryDto country);

        String DropCountryById(int countId);

        CountryDto GetCountryById(int countId);

        List<CountryDto> GetCountriesByName(string name);

    }
}
