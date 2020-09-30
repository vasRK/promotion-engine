using System.Linq;
using System.Collections.Generic;

namespace PromotionEngine.Models
{
	/// <summary>
	/// Represents a product in cart
	/// </summary>
	public class CartProduct
	{
		public Product Product { get; set; }

		public List<DiscountedProduct> DiscountedProducts { get; set; }

		/// <summary>
		/// Total count of product being ordered.
		/// </summary>
		public int Count { get; set; }

		private bool _IsPromoApplied { get; set; }

		public double GetActualPrice()
		{
			return this.Count * this.Product.Price;
		}

		/// <summary>
		/// Returns price after applying different promotions, if any
		/// </summary>
		/// <returns></returns>
		public double GetCartPrice()
		{
			if (this.DiscountedProducts.Count > 0)
			{
				var discountedProductCount = this.DiscountedProducts.Select(dp => dp.Count).Sum();
				var price = this.DiscountedProducts.Select(dp => dp.GetPrice()).Sum();

				//calculates correct price when all/some products are discounted
				return price + ((this.Count - discountedProductCount) * this.Product.Price);
			}

			//if no product is discounted the return actual price.
			return this.GetActualPrice();
		}

		//Replace this with calculate Cart price with a promotion?
		public void SetCartPrice()
		{

		}

		public double GetDiscount()
		{
			return 0.0;
		}

		public bool IsPromoApplied()
		{
			return this._IsPromoApplied;
		}

		public CartProduct()
		{
			DiscountedProducts = new List<DiscountedProduct>();
		}
	}
}
