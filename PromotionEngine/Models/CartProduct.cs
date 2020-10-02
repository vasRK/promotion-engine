using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Models
{
	/// <summary>
	/// Represents a product in cart
	/// </summary>
	public class CartProduct
	{
		public Product Product { get; set; }
		/// <summary>
		/// contains list of discounted products for this particluar product.
		/// </summary>
		public List<DiscountedProduct> DiscountedProducts { get; set; }

		/// <summary>
		/// Total count of product being ordered.
		/// </summary>
		public int Count { get; set; }

		private bool _IsPromoApplied { get; set; }

		/// <summary>
		/// returns price without a promotion
		/// </summary>
		/// <returns></returns>
		public double GetActualPrice()
		{
			return this.Count * this.Product.Price;
		}

		/// <summary>
		/// returns price after applying one or more promotion(s)
		/// </summary>
		/// <returns></returns>
		public double GetDiscountedPrice()
		{
			return this._discountedPrice;
		}

		public BillingDetails GetBillingDetails()
		{
			return new BillingDetails()
			{
				Product = this.Product,
				Count = this.Count,
				BillAmount = GetCartPrice(),
				Statement = GetPriceBreakupStatement(),
			};
		}

		public bool IsPromoApplied()
		{
			return this._IsPromoApplied;
		}

		public bool setPromoApplied(bool state)
		{
			return this._IsPromoApplied = state;
		}

		public CartProduct()
		{
			DiscountedProducts = new List<DiscountedProduct>();
		}

		private double _discountedPrice { get; set; }
		private double _cartPrice { get; set; }

		string GetPriceBreakupStatement()
		{
			//Ensure GetCartPrice is always calculated before calling this method.

			var price = this.GetActualPrice();
			var cartPrice = this._cartPrice;
			var savedAmount = price - cartPrice;
			var productInfo = string.Format("{0} x {1}", this.Product.SKU, this.Count);
			var billStmt = string.Format("Actual Price: ${0}, Final Price ${1}", price, cartPrice);

			return string.Format("{0} \n -> {1}", productInfo, billStmt);
		}

		/// <summary>
		/// Returns price after applying different promotions, if any
		/// </summary>
		/// <returns></returns>
		private double GetCartPrice()
		{
			if (this.DiscountedProducts.Count > 0)
			{
				var discountedProductCount = this.DiscountedProducts.Select(dp => dp.Count).Sum();
				var discountPrice = this.DiscountedProducts.Select(dp => dp.GetPrice()).Sum();

				this._discountedPrice = discountPrice;
				this._cartPrice = discountPrice + ((this.Count - discountedProductCount) * this.Product.Price);

				//calculates correct price when all/some products are discounted
				return _cartPrice;
			}

			//if no product is discounted then return actual price.
			var actualPrice = this.GetActualPrice();
			this._cartPrice = actualPrice;
			return actualPrice;
		}
	}
}
