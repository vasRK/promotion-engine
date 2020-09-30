using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionEngine.Models;

namespace PromotionEngine
{
	class Program
	{
		private static List<Product> _products;
		static void Main(string[] args)
		{
			var cart = new Cart();

			PopulateCartForSKUA(cart, 5);
			PopulateCartForSKUB(cart, 5);
			//PopulateCartForSKUC(cart, 1);
			//PopulateCartForSKUD(cart, 1);

			var promotions = GetPromotionForSKUA();
			promotions.AddRange(GetPromotionForSKUB());
			//promotions.AddRange(GetPromotionForSKUCD());

			foreach (var promotion in promotions)
			{
				promotion.ApplyPromotion(cart.CartProducts);
			}

			cart.GenerateCartStatement();

			Console.ReadLine();
		}

		static Cart PopulateCart()
		{
			//Populate Carte
			var cart = new Cart();
			//Randomly select how many diff products should be ordered.
			int totalProducts = new Random(4).Next();
			var productCountGen = new Random(10);
			var products = GetProducts();

			for (var ctr = 1; ctr <= totalProducts; ctr++)
			{
				//Randomly select how many of a product should be ordered.
				var countToOrder = productCountGen.Next();
				if (countToOrder > 0)
				{
					var product = products.Where(prod => prod.Id == ctr).FirstOrDefault();
					if (product != default(Product))
					{
						var cartProd = new CartProduct()
						{
							Product = product,
							Count = countToOrder
						};

						cart.AddCartProduct(cartProd);
					}
				}
			}

			return cart;
		}

		static List<Product> GetProducts()
		{
			if (Program._products == null)
			{
				var products = new List<Product>();
				products.Add(new Product() { Id = 1, SKU = Constants.SKUA, Price = 50 });
				products.Add(new Product() { Id = 2, SKU = Constants.SKUB, Price = 30 });
				products.Add(new Product() { Id = 3, SKU = Constants.SKUC, Price = 20 });
				products.Add(new Product() { Id = 4, SKU = Constants.SKUD, Price = 15 });

				Program._products = products;
			}

			return Program._products;
		}

		static List<Promotion> GetAllPromotions()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for A
			promotions.AddRange(GetPromotionForSKUA());

			//Promotion for B
			promotions.AddRange(GetPromotionForSKUB());

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

		#region Product A helpers




		static List<Promotion> GetPromotionForSKUA()
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
		static Cart PopulateCartForSKUA(Cart cart, int count)
		{
			//Populate Carte
			var products = GetProducts();
			var productA = products.Where(prod => prod.SKU == Constants.SKUA).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = count, Product = productA };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		#endregion

		#region Product B helpers

		static Cart PopulateCartForSKUB(Cart cart, int count)
		{
			//Populate Carte
			var products = GetProducts();
			var productB = products.Where(prod => prod.SKU == Constants.SKUB).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = count, Product = productB };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		static List<Promotion> GetPromotionForSKUB()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for A
			var promotionB = new Promotion() { Type = PromotionType.Single };
			var productB = products.Where(prod => prod.SKU == Constants.SKUB).FirstOrDefault();
			var promoProductB = new PromotionProduct() { Product = productB, ProductCount = 2, PriceMultiplier = 45 };
			promotionB.PromotionProducts.Add(promoProductB);
			promotions.Add(promotionB);

			return promotions;
		}

		#endregion

		#region Product C 

		static Cart PopulateCartForSKUC(Cart cart, int count)
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

		static Cart PopulateCartForSKUD(Cart cart, int count)
		{
			//Populate Carte
			var products = GetProducts();
			var productD = products.Where(prod => prod.SKU == Constants.SKUD).FirstOrDefault();
			var cartProduct = new CartProduct() { Count = count, Product = productD };

			cart.AddCartProduct(cartProduct);
			return cart;
		}

		#endregion
		static List<Promotion> GetPromotionForSKUCD()
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
