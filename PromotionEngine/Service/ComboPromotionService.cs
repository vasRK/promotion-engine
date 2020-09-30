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

			//Check all promo product are in cart products also
			var areEqual = promoSKUIds.ToHashSet().SetEquals(cartProdSKUIds.ToHashSet());

			if (areEqual)
			{
				//Can I apply promo code if comobo product count is diff 
				//e.g Combo offer (2 C's and 3 D's) this offer can not be applied if user order 1C and 4D as we do not meet criteria for C)
				var batchSizes = new List<int>();
				foreach (var skuId in promoSKUIds)
				{
					var cartProduct = cartProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
					var promotionProduct = promotion.PromotionProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
					var discountedProductCount = cartProduct.DiscountedProducts.Select(dp => dp.Count).Sum();
					var nonDiscountedCount = cartProduct.Count - discountedProductCount;

					//if not all products have a discount applied
					if (nonDiscountedCount > 0)
					{
						var numOfbatchs = (int)nonDiscountedCount / promotionProduct.ProductCount;
						batchSizes.Add(numOfbatchs);
					}
				}

				var minBatchSize = batchSizes.Min();
				if (batchSizes.Count == promoSKUIds.Count)
				{
					var cancelPromoApplication = false;
					foreach (var skuId in promoSKUIds)
					{
						var cartProduct = cartProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
						var promotionProduct = promotion.PromotionProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
						var discountedProductCount = cartProduct.DiscountedProducts.Select(dp => dp.Count).Sum();
						var nonDiscountedCount = cartProduct.Count - discountedProductCount;

						//if not all products have a discount applied
						if (nonDiscountedCount > 0)
						{
							//var numOfbatchs = (int)nonDiscountedCount / promotionProduct.ProductCount;
							var discountedProdCount = minBatchSize * promotionProduct.ProductCount;

							if (discountedProdCount > 0)
							{
								var disountedProduct = new DiscountedProduct()
								{
									Count = discountedProdCount,
									Product = cartProduct.Product,
									Promotion = promotion
								};

								cartProduct.DiscountedProducts.Add(disountedProduct);
							}
							else
							{
								cancelPromoApplication = true;
								break;
							}

							cartProduct.setPromoApplied(true);
						}
					}

					//Undo promo application and revert setPromoApplied
					if (cancelPromoApplication)
					{
						foreach (var skuId in promoSKUIds)
						{
							var cartProduct = cartProducts.Where(cp => cp.Product.SKU == skuId).FirstOrDefault();
							cartProduct.DiscountedProducts = cartProduct.DiscountedProducts.Where(dp => dp.Promotion.Id != promotion.Id).ToList();
							cartProduct.setPromoApplied(cartProduct.DiscountedProducts.Count > 0);
						}
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
