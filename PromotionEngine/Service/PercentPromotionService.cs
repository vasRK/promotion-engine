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
			var productBatchCount = (int)discountedProduct.Count / promotionProduct.ProductCount;
			return productBatchCount * promotionProduct.ProductCount * (1 - promotionProduct.PriceMultiplier) * promotionProduct.Product.Price;
		}
	}
}
