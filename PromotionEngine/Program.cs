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
		static void Main(string[] args)
		{
			var cart = PopulateCart();
			var promotions = GetPromotions();

			foreach (var promotion in promotions)
			{
				promotion.ApplyPromotion(cart.CartProducts);
			}

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
			var products = new List<Product>();
			products.Add(new Product() { Id = 1, SKU = "A", Price = 50 });
			products.Add(new Product() { Id = 2, SKU = "B", Price = 30 });
			products.Add(new Product() { Id = 3, SKU = "C", Price = 20 });
			products.Add(new Product() { Id = 4, SKU = "D", Price = 15 });

			return products;
		}

		static List<Promotion> GetPromotions()
		{
			var products = GetProducts();
			var promotions = new List<Promotion>();

			//Promotion for A
			var promotionA = new Promotion() { Type = PromotionType.Single };
			var productA = products.Where(prod => prod.SKU == "A").FirstOrDefault();
			var promoAProduct = new PromotionProduct() { Product = productA, ProductCount = 3 };
			promotionA.PromotionProducts.Add(promoAProduct);
			promotions.Add(promotionA);

			//Promotion for B
			var promotionB = new Promotion() { Type = PromotionType.Single };
			var productB = products.Where(prod => prod.SKU == "B").FirstOrDefault();
			var promoBProduct = new PromotionProduct() { Product = productB, ProductCount = 2 };
			promotionB.PromotionProducts.Add(promoBProduct);
			promotions.Add(promotionB);

			//Promotion for C & D
			var promotionCD = new Promotion() { Type = PromotionType.Combo };
			var productC = products.Where(prod => prod.SKU == "C").FirstOrDefault();
			var productD = products.Where(prod => prod.SKU == "D").FirstOrDefault();
			var promoCProduct = new PromotionProduct() { Product = productC, ProductCount = 1, PromotionPrice = 0 };
			var promoDProduct = new PromotionProduct() { Product = productD, ProductCount = 1, PromotionPrice = 30 };
			promotionCD.PromotionProducts.Add(promoCProduct);
			promotionCD.PromotionProducts.Add(promoDProduct);
			promotions.Add(promotionCD);

			return promotions;
		}
	}
}
