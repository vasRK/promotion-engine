using System.Collections.Generic;
using PromotionEngine.Models;
using System.Linq;

namespace PromotionEngine.Service
{
	public sealed class PercentPromotionService : SinglePromotionService
	{
		public override double CalculateDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct)
		{
			//price is calculated for a batch on N
			var productCount = (int)discountedProduct.Count / promotionProduct.ProductCount;
			return productCount * promotionProduct.ProductCount * (1 - promotionProduct.PriceMultiplier) * promotionProduct.Product.Price;
		}
	}
}
