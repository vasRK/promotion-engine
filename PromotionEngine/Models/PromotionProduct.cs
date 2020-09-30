namespace PromotionEngine.Models
{
	public class PromotionProduct
	{
		public Product Product { get; set; }

		/// <summary>
		/// tells how many products need to make a promo valid.
		/// </summary>
		public int ProductCount { get; set; }

		public double PriceMultiplier { get; set; }
	}
}
