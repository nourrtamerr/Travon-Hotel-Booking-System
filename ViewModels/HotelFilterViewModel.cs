using System.ComponentModel.DataAnnotations;

namespace MVCBookingFinal_YARAB_.ViewModels
{
	public class HotelFilterViewModel
	{
		public AmenityEnum Amenities { set; get; }
		//public AmenityEnum amenity { set; get; }
		public HotelsortBy Sorting { set; get; }
	}
	public enum HotelsortBy
	{
        [Display(Name = "Our Top Picks")]
        OurTopPicks,


        [Display(Name = "Stars Rating")]
        StarsRating,


        [Display(Name = "Reviews Rating")]
        ReviewsRating

	}
}
