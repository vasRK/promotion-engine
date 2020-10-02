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
		public void ProductA_WhenPromotionApplied_Should_Crate_DiscountedProducts()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUA(cart);

			var promotions = GetPromotionForSKUA();
			var singlePromoService = (IPromotionService)new PromotionService();
			var singlePromo = promotions.Where(promo => promo.Type == PromotionType.Single).FirstOrDefault();

			//Act
			singlePromoService.ApplyPromotion(singlePromo, cart.CartProducts);

			//Assert
			var cartProductA = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUA).FirstOrDefault();
			Assert.IsTrue(cartProductA.IsPromoApplied());
			Assert.IsTrue(cartProductA.DiscountedProducts.Sum(dp => dp.Count) == 3);
		}

		[TestMethod]
		public void ProductA_WhenPromotionAppliedAndGetDiscountedPriceCalled_Should_GetCorrectDiscountedPrice()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUA(cart);

			var promotions = GetPromotionForSKUA();
			var singlePromoService = (IPromotionService)new PromotionService();
			var singlePromo = promotions.Where(promo => promo.Type == PromotionType.Single).FirstOrDefault();
			var cartProductA = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUA).FirstOrDefault();

			//all products discounted
			cartProductA.Count = 6;

			//Act
			singlePromoService.ApplyPromotion(singlePromo, cart.CartProducts);
			var discountedPrice = singlePromoService.CalculateDiscountedPrice(singlePromo.PromotionProducts.First()
				, cartProductA.DiscountedProducts.First());

			//Assert
			Assert.IsTrue(discountedPrice == 260.0);
		}

		[TestMethod]
		public void ProductB_WhenPromotionAppliedAndGetDiscountedPriceCalled_Should_GetCorrectDiscountedPrice()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUB(cart);

			var promotions = GetPromotionForSKUB();
			var singlePromoService = (IPromotionService)new PromotionService();
			var singlePromo = promotions.Where(promo => promo.Type == PromotionType.Single).FirstOrDefault();
			var cartProductB = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUB).FirstOrDefault();

			//Act
			singlePromoService.ApplyPromotion(singlePromo, cart.CartProducts);
			var discountedPrice = singlePromoService.CalculateDiscountedPrice(singlePromo.PromotionProducts.First()
				, cartProductB.DiscountedProducts.First());

			//Assert
			//Only six products out of total 7, two costing 45
			Assert.IsTrue(discountedPrice == 135.0);
		}

		#region Product A helpers

		List<Promotion> GetPromotionForSKUA()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for A
			var promotionA = new Promotion() { Type = PromotionType.Single, Id = 1001 };
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
		Cart PopulateCartForSKUA(Cart cart)
		{
			//Populate Carte
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

		#region Product B helpers

		Cart PopulateCartForSKUB(Cart cart)
		{
			//Populate Carte
			var products = GetProducts();
			var productB = products.Where(prod => prod.SKU == Constants.SKUB).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = 7, Product = productB };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		List<Promotion> GetPromotionForSKUB()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for B
			var promotionB = new Promotion() { Type = PromotionType.Single, Id = 1002 };
			var productB = products.Where(prod => prod.SKU == Constants.SKUB).FirstOrDefault();
			var promoProductB = new PromotionProduct() { Product = productB, ProductCount = 2, PriceMultiplier = 45 };
			promotionB.PromotionProducts.Add(promoProductB);
			promotions.Add(promotionB);

			return promotions;
		}

		#endregion
	}
}
