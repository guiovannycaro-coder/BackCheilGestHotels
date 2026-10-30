using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;

namespace BackCheilGestHotels.application.services
{
    public class CitiesService : ICitiesService
    {
        private readonly ICityRepository _citiesRepository;

        public CitiesService(ICityRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        // DELETE: Eliminar ciudad por ID
        public bool DropCityById(int dato)
        {
            try
            {
                string result = _citiesRepository.DropCityById(dato);

                return result == "City deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error deleting city.",
                    ex);
            }
        }

        // GET: Obtener todas las ciudades
        public List<CityDto> GetCities()
        {
            try
            {
                return _citiesRepository.GetCities();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving cities.",
                    ex);
            }
        }

        // GET: Obtener ciudad por ID
        public List<CityDto> GetCityById(int dato)
        {
            try
            {
                CityDto city = _citiesRepository.GetCityById(dato);

                if (city == null)
                {
                    return new List<CityDto>();
                }

                return new List<CityDto> { city };
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error retrieving city by ID.",
                    ex);
            }
        }

        // GET: Buscar ciudades por nombre
        public List<CityDto> GetCityByName(string dato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return new List<CityDto>();
                }

                return _citiesRepository.GetCitiesByName(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error searching cities by name.",
                    ex);
            }
        }

        // POST: Crear ciudad
        public int InsCity(CityDto city)
        {
            try
            {
                string result = _citiesRepository.InsCity(city);

                const string prefix = "City created successfully. ID: ";

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
                    "Error creating city.",
                    ex);
            }
        }

        // PUT: Actualizar ciudad
        public bool UpdCity(CityDto city)
        {
            try
            {
                string result = _citiesRepository.UpdCity(city);

                return result == "City updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "Error updating city.",
                    ex);
            }
        }
    }
}
