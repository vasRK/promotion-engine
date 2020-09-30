using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine.Models;
using PromotionEngine.Service;

namespace PromoEngine.Tests
{
	[TestClass]
	public class PercentPromotionServiceTests
	{
		[TestMethod]
		public void WhenPromotionApplied_Should_CrateCorrectNumberOfDiscountedProducts()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUD(cart, 20);

			var promotions = GetPromotionForSKUD();
			var promoService = (IPromotionService)new PercentPromotionService();
			var percentPromo = promotions.Where(promo => promo.Type == PromotionType.Percent).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(percentPromo, cart.CartProducts);
			var cartProductD = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUD).FirstOrDefault();
			var discountedPrice = promoService.CalculateDiscountedPrice(percentPromo.PromotionProducts.First(), cartProductD.DiscountedProducts.First());

			//Assert
			Assert.IsTrue(cartProductD.IsPromoApplied());
			Assert.IsTrue(cartProductD.DiscountedProducts.Sum(dp => dp.Count) == 20);
			Assert.IsTrue(discountedPrice == 270.0);
		}

		[TestMethod]
		public void WhenOrderProductCountIsLessThan_PromoProductSize_Should_CalculateCorrectTotal()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUD(cart, 9);

			var promotions = GetPromotionForSKUD();
			var promoService = (IPromotionService)new PercentPromotionService();
			var percentPromo = promotions.Where(promo => promo.Type == PromotionType.Percent).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(percentPromo, cart.CartProducts);
			var cartProductD = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUD).FirstOrDefault();

			//Assert
			Assert.IsFalse(cartProductD.IsPromoApplied());
			Assert.IsTrue(cartProductD.DiscountedProducts.Sum(dp => dp.Count) == 0);
		}

		List<Product> GetProducts()
		{
			var products = new List<Product>();
			products.Add(new Product() { Id = 3, SKU = Constants.SKUC, Price = 20 });
			products.Add(new Product() { Id = 4, SKU = Constants.SKUD, Price = 15 });

			return products;
		}


		#region Product D 

		Cart PopulateCartForSKUD(Cart cart, int count)
		{
			//Populate Carte
			var products = GetProducts();
			var productD = products.Where(prod => prod.SKU == Constants.SKUD).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = count, Product = productD };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		#endregion
		List<Promotion> GetPromotionForSKUD()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for C & D
			var promotionCD = new Promotion() { Type = PromotionType.Percent, Id = 1004 };
			var productD = products.Where(prod => prod.SKU == Constants.SKUD).FirstOrDefault();
			var promoDProduct = new PromotionProduct() { Product = productD, ProductCount = 10, PriceMultiplier = .10 };
			promotionCD.PromotionProducts.Add(promoDProduct);
			promotions.Add(promotionCD);

			return promotions;
		}
	}
}
