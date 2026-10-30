using BackCheilGestHotels.application.entities;

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
    }
}
