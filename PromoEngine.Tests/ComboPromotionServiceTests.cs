using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine.Models;
using PromotionEngine.Service;

namespace PromoEngine.Tests
{
	[TestClass]
	public class ComboPromotionServiceTests
	{
		[TestMethod]
		public void WhenPromotionApplied_Should_CrateCorrectNumberOfDiscountedProducts()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUC(cart, 2);
			PopulateCartForSKUD(cart, 3);

			var promotions = GetPromotionForSKUCD();
			var promoService = (IPromotionService)new ComboPromotionService();
			var comboPromo = promotions.Where(promo => promo.Type == PromotionType.Combo).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(comboPromo, cart.CartProducts);

			//Assert
			var cartProductC = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var cartProductD = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUD).FirstOrDefault();
			Assert.IsTrue(cartProductC.IsPromoApplied());
			Assert.IsTrue(cartProductD.IsPromoApplied());
			Assert.IsTrue(cartProductC.DiscountedProducts.Sum(dp => dp.Count) == 2);
			Assert.IsTrue(cartProductD.DiscountedProducts.Sum(dp => dp.Count) == 2);
		}

		[TestMethod]
		public void WhenPromotionAppliedAndGetDiscountedPriceCalled_Should_GetCorrectDiscountedPrice()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUC(cart, 2);
			PopulateCartForSKUD(cart, 2);

			var promotions = GetPromotionForSKUCD();
			var promoService = (IPromotionService)new ComboPromotionService();
			var comboPromo = promotions.Where(promo => promo.Type == PromotionType.Combo).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(comboPromo, cart.CartProducts);

			//Discounted Price for C.
			var cartProductC = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var promoProductC = comboPromo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var discountedPriceC = promoService.CalculateDiscountedPrice(promoProductC, cartProductC.DiscountedProducts.First());

			//Discounted Price for D.
			var cartProductD = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUD).FirstOrDefault();
			var promoProductD = comboPromo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUD).FirstOrDefault();
			var discountedPriceD = promoService.CalculateDiscountedPrice(promoProductD, cartProductD.DiscountedProducts.First());

			//Assert
			Assert.IsTrue(discountedPriceC == 0.0);
			Assert.IsTrue(discountedPriceD == 60.0);
		}

		[TestMethod]
		public void ProductC_WhenOnlyOneProductOrdered__DiscountedProducts_Should_BeZero()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUC(cart, 2);

			var promotions = GetPromotionForSKUCD();
			var promoService = (IPromotionService)new ComboPromotionService();
			var comboPromo = promotions.Where(promo => promo.Type == PromotionType.Combo).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(comboPromo, cart.CartProducts);

			//Discounted Price for C.
			var cartProductC = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var promoProductC = comboPromo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUC).FirstOrDefault();

			//Assert
			Assert.IsTrue(cartProductC.DiscountedProducts.Count == 0);
		}

		[TestMethod]
		public void WhenPromotionProductWithVaringSize_Should_ApplyPromotionToMinimumPossibleBatches()
		{
			//Setup
			var cart = new Cart();
			PopulateCartForSKUC(cart, 7);
			PopulateCartForSKUD(cart, 20);

			var promotions = GetPromotionForSKUCD();
			var promo = promotions.First();
			var promoProdD = promo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUD).First();
			promoProdD.ProductCount = 4;

			var promoService = (IPromotionService)new ComboPromotionService();
			var comboPromo = promotions.Where(pro => pro.Type == PromotionType.Combo).FirstOrDefault();

			//Act
			promoService.ApplyPromotion(comboPromo, cart.CartProducts);

			//Discounted Price for C.
			var cartProductC = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var promoProductC = comboPromo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUC).FirstOrDefault();
			var discountedPriceC = promoService.CalculateDiscountedPrice(promoProductC, cartProductC.DiscountedProducts.First());

			//Discounted Price for D.
			var cartProductD = cart.CartProducts.Where(cp => cp.Product.SKU == Constants.SKUD).FirstOrDefault();
			var promoProductD = comboPromo.PromotionProducts.Where(pp => pp.Product.SKU == Constants.SKUD).FirstOrDefault();
			var discountedPriceD = promoService.CalculateDiscountedPrice(promoProductD, cartProductD.DiscountedProducts.First());

			//Assert
			Assert.IsTrue(cartProductC.DiscountedProducts.Sum(dp => dp.Count) == 5);
			Assert.IsTrue(discountedPriceC == 0.0);
			Assert.IsTrue(discountedPriceD == 150.0);
		}


		List<Product> GetProducts()
		{
			var products = new List<Product>();
			products.Add(new Product() { Id = 3, SKU = Constants.SKUC, Price = 20 });
			products.Add(new Product() { Id = 4, SKU = Constants.SKUD, Price = 15 });

			return products;
		}

		#region Product C 

		Cart PopulateCartForSKUC(Cart cart, int count)
		{
			//Populate Carte
			var products = GetProducts();
			var productC = products.Where(prod => prod.SKU == Constants.SKUC).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = count, Product = productC };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		#endregion

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
		List<Promotion> GetPromotionForSKUCD()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for C & D
			var promotionCD = new Promotion() { Type = PromotionType.Combo };
			var productC = products.Where(prod => prod.SKU == Constants.SKUC).FirstOrDefault();
			var productD = products.Where(prod => prod.SKU == Constants.SKUD).FirstOrDefault();
			var promoCProduct = new PromotionProduct() { Product = productC, ProductCount = 1, PriceMultiplier = 0 };
			var promoDProduct = new PromotionProduct() { Product = productD, ProductCount = 1, PriceMultiplier = 30 };
			promotionCD.PromotionProducts.Add(promoCProduct);
			promotionCD.PromotionProducts.Add(promoDProduct);
			promotions.Add(promotionCD);

			return promotions;
		}
	}
}
