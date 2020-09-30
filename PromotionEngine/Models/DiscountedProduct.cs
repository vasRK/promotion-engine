using System.Linq;

namespace PromotionEngine.Models
{
	/// <summary>
	/// Represents a product after a certain promotion applied.
	/// </summary>
	public class DiscountedProduct
	{
		public Product Product { get; set; }

		/// <summary>
		/// For how many counts of this product promotion is applied.
		/// for e.g user can order 10 of a product but promotion is applied to 6
		/// </summary>
		public int Count { get; set; }

		public Promotion Promotion { get; set; }

		public double GetPrice()
		{
			return this.Promotion.GetDiscountedPrice(this.Product, this.Count);
		}
	}
}
