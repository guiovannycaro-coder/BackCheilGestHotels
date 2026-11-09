using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.Domain.Models;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IHotelRepository
    {
        List<HotelDto> GetHotels();

        HotelDto GetHotelById(int hotId);

        List<HotelDto> GetHotelsByName(string name);

        List<HotelDto> GetHotelsByCityId(int cityId);
        List<HotelDto> searchHotels(string? Name, int? Rating);

        String InsHotel(HotelDto hotel);

        String UpdHotel(HotelDto hotel);

        String DropHotelById(int hotId);

        public List<Hotel> GetHotelsByRating(int rating);
    }
}
