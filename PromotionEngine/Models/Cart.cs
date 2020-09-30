using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine.Models
{
	public class Cart
	{
		public List<CartProduct> CartProducts { get; set; }

		public Cart()
		{
			CartProducts = new List<CartProduct>();
		}

		public bool AddCartProduct(CartProduct cartProduct)
		{
			this.CartProducts.Add(cartProduct);

			return true;
		}
	}
}
