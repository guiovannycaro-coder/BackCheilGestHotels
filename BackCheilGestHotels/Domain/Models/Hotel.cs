using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.Models
{
    public class Hotel
    {
        public int HotId { get; set; }

        public string HotName { get; set; } 

        public string HotDescription { get; set; } 

        public string HotAddress { get; set; }

        public string HotPhone { get; set; }

        public string HotEmail { get; set; }

        public int HotCityId { get; set; }

        public int HotRating { get; set; }

        public string HotRateSign { get; set; }

        public bool HotIsActive { get; set; }

        public string HotImages { get; set; }
        public CityDto City { get; set; }
    }
}
