using MVCBookingFinal_YARAB_.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBookingFinal_YARAB_.Models
{
	public class Amenity
	{

		public int Id { set; get; }
		[MinLength(3)]
		[EnumDataType(typeof(AmenityEnum))]
		public AmenityEnum Amenities { set; get; }
		[NotMapped]
		[DataType(DataType.Currency)]
		public double Price => AmenityFixedPrices.GetTotalPrice(this.Amenities);
		public virtual List<Reservation> Reservation { set; get; }
		public virtual List<Hotel> Hotels { set; get; }
	
	}

    [Flags]
    public enum AmenityEnum
    {
      
        [Display(Name = "Airport Shuttle")]
        AirportShuttle = 1,
        Parking = 2,
        [Display(Name = "Pet friendly")]
        PetFriendly = 4,
        Gym = 8,
        Spa = 16,
        Restaurant = 32,
        Laundry = 64,
        [Display(Name = "Extra Room Services")]
        ExtraRoomService = 128
    }

}
