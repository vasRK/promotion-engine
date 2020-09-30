using System.Collections.Generic;
using PromotionEngine.Models;
using System.Linq;

namespace PromotionEngine.Service
{
	public sealed class ComboPromotionService : IPromotionService
	{
		void IPromotionService.ApplyPromotion(Promotion promotion, List<CartProduct> cartProducts)
		{
			var promoSKUIds = promotion.PromotionProducts.Select(pp => pp.Product.SKU).ToList();
			var cartProductList = cartProducts.Where(cp => !cp.IsPromoApplied() && promoSKUIds.Contains(cp.Product.SKU)).ToList();
			var cartProdSKUIds = cartProductList.Select(cp => cp.Product.SKU).ToList();
			var minCount = cartProducts.Select(promoProd => promoProd.Count).Min();

			//Check all promo product are in cart products 
			var areEqual = promoSKUIds.ToHashSet().SetEquals(cartProdSKUIds.ToHashSet());
			if (areEqual)
			{
				foreach (var skuId in promoSKUIds)
				{
					var cartProduct = cartProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
					var promoProduct = promotion.PromotionProducts.Where(promoProd => promoProd.Product.SKU == skuId).FirstOrDefault();
					var discountedProductCount = cartProduct.DiscountedProducts.Select(dp => dp.Count).Sum();
					var nonDiscountedCount = cartProduct.Count - discountedProductCount;

					//if not all products have a discount applied
					if (nonDiscountedCount > 0)
					{
						var numOfbatchs = (int)nonDiscountedCount / minCount;
						var disountedProduct = new DiscountedProduct()
						{
							Count = numOfbatchs * minCount,
							Product = cartProduct.Product,
							Promotion = promotion
						};

						cartProduct.setPromoApplied(true);
						cartProduct.DiscountedProducts.Add(disountedProduct);
					}
				}
			}
		}

		double IPromotionService.CalculateDiscountedPrice(PromotionProduct promotionProduct, DiscountedProduct discountedProduct)
		{
			//price is calculated for a batch on N
			var productCount = (int)discountedProduct.Count / promotionProduct.ProductCount;
			return productCount * promotionProduct.PriceMultiplier;
		}
	}
}
