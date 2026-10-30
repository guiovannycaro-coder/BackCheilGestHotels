namespace BackCheilGestHotels.application.entities
{
    public class HotelDto
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

        public string hotImages { get; set; }


        public CityDto City { get; set; }
    }
}
