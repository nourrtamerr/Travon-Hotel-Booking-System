using MVCBookingFinal_YARAB_.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBookingFinal_YARAB_.Helpers
{
	public static class AmenityFixedPrices
	{
		[NotMapped]
		public static readonly Dictionary<AmenityEnum, double> Prices = new()
		{
			{ AmenityEnum.PetFriendly, 0 },   // Free
			{ AmenityEnum.Parking, 150 },
			{ AmenityEnum.AirportShuttle, 200 },
			{ AmenityEnum.Gym, 150 },
			{ AmenityEnum.Spa, 300 },
			{ AmenityEnum.Restaurant, 250 },
			{ AmenityEnum.ExtraRoomService, 100 },
			{ AmenityEnum.Laundry, 120 }
		};
		public static double GetTotalPrice(AmenityEnum amenity)
		{
			return Prices.Where(kv => amenity.HasFlag(kv.Key) && (kv.Key != AmenityEnum.AirportShuttle)).Sum(kv => kv.Value);
		}
	}
}
