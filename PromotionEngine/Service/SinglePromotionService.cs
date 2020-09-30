using System.Collections.Generic;
using PromotionEngine.Models;
using System.Linq;

namespace PromotionEngine.Service
{
	public sealed class SinglePromotionService : IPromotionService
	{
		void IPromotionService.ApplyPromotion(Promotion promotion, List<CartProduct> cartProducts)
		{
			//Single promo so apply to a single product
			var promoProduct = promotion.PromotionProducts.First();
			if (promoProduct != default(PromotionProduct))
			{
				var cartProduct = cartProducts.Where(cp => cp.Product.SKU == promoProduct.Product.SKU && !cp.IsPromoApplied()).First();
				if (cartProduct != default(CartProduct))
				{
					var discountedProductCount = cartProduct.DiscountedProducts.Select(dp => dp.Count).Sum();
					var nonDiscountedCount = cartProduct.Count - discountedProductCount;

					//if not all products have a discount applied
					if (nonDiscountedCount > 0)
					{
						var numOfbatchs = (int)nonDiscountedCount / promoProduct.ProductCount;
						var disountedProduct = new DiscountedProduct()
						{
							Count = numOfbatchs * promoProduct.ProductCount,
							Product = cartProduct.Product,
							Promotion = promotion
						};

						cartProduct.setPromoApplied(true);
						cartProduct.DiscountedProducts.Add(disountedProduct);
					}
				}
			}
		}


		double IPromotionService.GetDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct)
		{
			//price is calculated for a batch on N
			var productCount = (int)discountedProduct.Count / promotionProduct.ProductCount;
			return productCount * promotionProduct.PriceMultiplier;
		}
	}
}
