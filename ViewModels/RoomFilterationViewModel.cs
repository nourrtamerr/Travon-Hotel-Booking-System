using System.ComponentModel.DataAnnotations;

namespace MVCBookingFinal_YARAB_.ViewModels
{
	public class RoomFilterationViewModel
	{
		public decimal minPrice { set; get; }
		public decimal maxPrice { set; get; }
		//public AmenityEnum amenity { set; get; }
		public sortBy Sorting { set; get; }
	}
	public enum sortBy
	{
        [Display(Name = "Our Top Picks")]
        OurTopPicks,

        [Display(Name = "Low Price")]
        LowPrice,

        [Display(Name = "High Price")]
        HighPrice

	}
}
