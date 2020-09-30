using System.Collections.Generic;
using PromotionEngine.Models;

namespace PromotionEngine.Service
{
	public sealed class ComboPromotionService : IPromotionService
	{
		void IPromotionService.ApplyPromotion(Promotion promotion, List<CartProduct> cartProducts)
		{
			throw new System.NotImplementedException();
		}

		double IPromotionService.CalculateDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct)
		{
			throw new System.NotImplementedException();
		}
	}
}
