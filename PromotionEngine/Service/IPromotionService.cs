using System.Collections.Generic;
using PromotionEngine.Models;

namespace PromotionEngine.Service
{
	/// <summary>
	/// Service to apply and calculated promotion price of a product
	/// </summary>
	public interface IPromotionService
	{
		/// <summary>
		/// Applies a given promotion to a product in cart
		/// </summary>
		/// <param name="promotion"></param>
		/// <param name="cartProducts"></param>
		void ApplyPromotion(Promotion promotion, List<CartProduct> cartProducts);

		/// <summary>
		/// calculates and returns cart value of a product after promotion discount
		/// </summary>
		/// <param name="promotionProduct"></param>
		/// <param name="discountedProduct"></param>
		/// <returns></returns>
		double CalculateDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct);
	}
}
