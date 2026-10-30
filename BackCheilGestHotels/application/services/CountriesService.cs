using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;

namespace BackCheilGestHotels.application.services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // DELETE: Eliminar país por ID
        public bool DropCountryById(int dato)
        {
            try
            {
                string result = _countryRepository.DropCountryById(dato);

                return result == "Country deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting country.",
                    ex);
            }
        }

        // GET: Obtener todos los países
        public List<CountryDto> GetCountries()
        {
            try
            {
                return _countryRepository.GetCountries();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving countries.",
                    ex);
            }
        }

        // GET: Obtener país por ID
        public List<CountryDto> GetCountryById(int dato)
        {
            try
            {
                CountryDto country = _countryRepository.GetCountryById(dato);

                if (country == null)
                {
                    return new List<CountryDto>();
                }

                return new List<CountryDto> { country };
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving country by ID.",
                    ex);
            }
        }

        // GET: Buscar países por nombre
        public List<CountryDto> GetCountryByName(string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return new List<CountryDto>();
                }

                return _countryRepository.GetCountriesByName(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error searching countries by name.",
                    ex);
            }
        }

        // POST: Crear país
        public int InsCountry(CountryDto country)
        {
            try
            {
                string result = _countryRepository.InsCountry(country);

                const string prefix = "Country created successfully. ID: ";

                if (result.StartsWith(prefix) &&
                    int.TryParse(
                        result.Substring(prefix.Length),
                        out int newId))
                {
                    return newId;
                }

                return 0;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error creating country.",
                    ex);
            }
        }

        // PUT: Actualizar país
        public bool UpdCountry(CountryDto country)
        {
            try
            {
                string result = _countryRepository.UpdCountry(country);

                return result == "Country updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating country.",
                    ex);
            }
        }
    }
}
