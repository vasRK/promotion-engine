using PromotionEngine.Models;

namespace PromotionEngine.Service
{
	/// <summary>
	/// Handles promotion for a single product, where promotion is applied as a percent of product price on purchashing N items
	/// </summary>
	public sealed class PercentPromotionService : PromotionService
	{
		/// <summary>
		/// Price calculation is diff from base, so overriding
		/// </summary>
		/// <param name="promotionProduct"></param>
		/// <param name="discountedProduct"></param>
		/// <returns></returns>
		public override double CalculateDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct)
		{
			//price is calculated for a batch of N products
			var productBatchCount = (int)discountedProduct.Count / promotionProduct.ProductCount;
			return discountedProduct.Count * (1 - promotionProduct.PriceMultiplier) * promotionProduct.Product.Price;
		}
	}
}
