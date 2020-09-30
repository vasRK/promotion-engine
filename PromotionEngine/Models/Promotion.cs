using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Models.Service;

namespace PromotionEngine.Models
{
	public class Promotion
	{
		public List<PromotionProduct> PromotionProducts { get; set; }

		public PromotionType Type { get; set; }

		//Is this needed here
		public double PromotionPrice { get; set; }

		public Promotion()
		{
			PromotionProducts = new List<PromotionProduct>();
		}

		public void ApplyPromotion(List<CartProduct> cartProducts)
		{
			var promoService = GetService();
			promoService.ApplyPromotion(cartProducts);
		}

		public double GetDiscountedPrice(Product product, int count)
		{
			var promoProduct = this.PromotionProducts.Where(prod => prod.Product.SKU == product.SKU).FirstOrDefault();
			var productCount = (int)count / promoProduct.ProductCount;

			return productCount * promoProduct.PromotionPrice;
		}


		/// <summary>
		/// For now service is created each time, there are other better ways 
		/// to do it like using DI or a Factory pattern etc.
		/// </summary>
		/// <returns></returns>
		private IPromotionService GetService()
		{
			var service = default(IPromotionService);
			switch (this.Type)
			{
				case PromotionType.Single:
					service = new SinglePromotionService();
					break;
				case PromotionType.Combo:
					service = new ComboPromotionService();
					break;
					//case PromotionType.Percent:
					//	break;
			}

			return service;
		}
	}

	public enum PromotionType
	{
		Single = 1,
		Combo = 2,
		Percent = 3
	}
}
