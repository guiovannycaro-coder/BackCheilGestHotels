using BackCheilGestHotels.application.entities;
using Microsoft.AspNetCore.Mvc;

namespace BackCheilGestHotels.application.iservices
{
    public interface IHotelsService
    {
        List<HotelDto> GetHotels();
        int InsHotel(HotelDto hotel);
        bool UpdHotel(HotelDto hotel);
        bool DropHotelById(int dato);
        List<HotelDto> GetHotelById(int dato);
        List<HotelDto> GetHotelByName(string dato);
      
        List<HotelDto> BuscarHotels(string? Name, int? Rating);
    }
}
