using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine.Models;
using PromotionEngine.Service;

namespace PromoEngine.Tests
{
	[TestClass]
	public class SinglePromotionServiceTests
	{
		[TestMethod]
		public void WhenPromotionApplied_Should_Crate_DiscountedProducts()
		{
			//Setup
			var cart = PopulateCartForSKUA();
			var promotions = GetPromotionForSKUA();
			var singlePromoService = (IPromotionService)new SinglePromotionService();
			var singlePromo = promotions.Where(promo => promo.Type == PromotionType.Single).FirstOrDefault();

			//Act
			singlePromoService.ApplyPromotion(singlePromo, cart.CartProducts);

			//Assert
			var cartProductA = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUA).FirstOrDefault();
			Assert.IsTrue(cartProductA.IsPromoApplied());
			Assert.IsTrue(cartProductA.DiscountedProducts.Sum(dp => dp.Count) == 3);
		}

		[TestMethod]
		public void WhenPromotionAppliedAndGetDiscountedPriceCalled_Should_GetdiscountedPrice()
		{
			//Setup
			var cart = PopulateCartForSKUA();
			var promotions = GetPromotionForSKUA();
			var singlePromoService = (IPromotionService)new SinglePromotionService();
			var singlePromo = promotions.Where(promo => promo.Type == PromotionType.Single).FirstOrDefault();

			//Act
			singlePromoService.ApplyPromotion(singlePromo, cart.CartProducts);
			var cartProductA = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUA).FirstOrDefault();
			var discountedProduct = cartProductA.DiscountedProducts.First();
			var discountedPrice = singlePromoService.CalculateDiscountedPrice(singlePromo.PromotionProducts.First(), discountedProduct);

			//Assert
			Assert.IsTrue(discountedPrice == 130.0);
		}

		#region Product A helpers

		List<Promotion> GetPromotionForSKUA()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for A
			var promotionA = new Promotion() { Type = PromotionType.Single };
			var productA = products.Where(prod => prod.SKU == Constants.SKUA).FirstOrDefault();
			var promoAProduct = new PromotionProduct() { Product = productA, ProductCount = 3, PriceMultiplier = 130 };
			promotionA.PromotionProducts.Add(promoAProduct);
			promotions.Add(promotionA);

			return promotions;
		}

		/// <summary>
		/// To test Product A
		/// </summary>
		/// <returns></returns>
		Cart PopulateCartForSKUA()
		{
			//Populate Carte
			var cart = new Cart();
			var products = GetProducts();
			var productA = products.Where(prod => prod.SKU == Constants.SKUA).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = 5, Product = productA };

			cart.AddCartProduct(cartProduct);

			return cart;
		}

		List<Product> GetProducts()
		{
			var products = new List<Product>();
			products.Add(new Product() { Id = 1, SKU = Constants.SKUA, Price = 50 });
			products.Add(new Product() { Id = 2, SKU = Constants.SKUB, Price = 30 });
			products.Add(new Product() { Id = 3, SKU = Constants.SKUC, Price = 20 });
			products.Add(new Product() { Id = 4, SKU = Constants.SKUD, Price = 15 });

			return products;
		}

		#endregion
	}
}
