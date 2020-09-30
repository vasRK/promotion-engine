using System.Collections.Generic;
using PromotionEngine.Models;

namespace PromotionEngine.Service
{
	public interface IPromotionService
	{
		void ApplyPromotion(List<CartProduct> cartProducts);
	}
}
