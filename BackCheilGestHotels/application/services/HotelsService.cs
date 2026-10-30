using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.Models;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;
using BackCheilGestHotels.infra.repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackCheilGestHotels.application.services
{
    public class HotelsService : IHotelsService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelsService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public List<HotelDto> BuscarHotels(string? Name, int? Rating)
        {
            try
            {
                List<HotelDto> hotels = _hotelRepository.searchHotels(Name, Rating);

                if (hotels == null)
                {
                    return new List<HotelDto>();
                }

                return hotels;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving the hotel by ID.",
                    ex);
            }
        }

       

        // Eliminar hotel por ID
        public bool DropHotelById(int dato)
        {
            try
            {
                string result = _hotelRepository.DropHotelById(dato);

                return result == "Hotel deleted successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while deleting the hotel.",
                    ex);
            }
        }

        // Consultar hotel por ID
        public List<HotelDto> GetHotelById(int dato)
        {
            try
            {
                HotelDto hotel = _hotelRepository.GetHotelById(dato);

                if (hotel == null)
                {
                    return new List<HotelDto>();
                }

                return new List<HotelDto> { hotel };
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving the hotel by ID.",
                    ex);
            }
        }

        // Buscar hoteles por nombre
        public List<HotelDto> GetHotelByName(string dato)
        {
            try
            {
                return _hotelRepository.GetHotelsByName(dato);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while searching hotels by name.",
                    ex);
            }
        }

        // Consultar todos los hoteles
        public List<HotelDto> GetHotels()
        {
            try
            {
                return _hotelRepository.GetHotels();
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving hotels.",
                    ex);
            }
        }

        // Insertar hotel
        public int InsHotel(HotelDto hotel)
        {
            try
            {
                string result = _hotelRepository.InsHotel(hotel);

                const string prefix = "Hotel created successfully. ID: ";

                if (result.StartsWith(prefix))
                {
                    string idText = result.Substring(prefix.Length);

                    if (int.TryParse(idText, out int hotelId))
                    {
                        return hotelId;
                    }
                }

                throw new BackCheilGestHotelException(
                    500,
                    "The hotel was not created successfully.",
                    new Exception("The repository returned an unexpected result."));
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while creating the hotel.",
                    ex);
            }
        }

        // Actualizar hotel
        public bool UpdHotel(HotelDto hotel)
        {
            try
            {
                string result = _hotelRepository.UpdHotel(hotel);

                return result == "Hotel updated successfully.";
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while updating the hotel.",
                    ex);
            }
        }
    }
}
