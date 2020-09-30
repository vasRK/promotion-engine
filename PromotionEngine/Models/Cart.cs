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

		public double CartValue { get; set; }

		public Cart()
		{
			CartProducts = new List<CartProduct>();
		}

		public bool AddCartProduct(CartProduct cartProduct)
		{
			this.CartProducts.Add(cartProduct);

			return true;
		}

		public void GenerateCartStatement()
		{
			var billDetailsList = new List<BillingDetails>();
			foreach (var cp in this.CartProducts)
			{
				billDetailsList.Add(cp.GetBillingDetails());
			}

			var ctr = 1;
			foreach (var bill in billDetailsList)
			{
				Console.WriteLine(string.Format("#SN {0} {1}", ctr++, bill.Statement));
			}

			Console.WriteLine("==============");
			Console.WriteLine("Total {0}", billDetailsList.Sum(bill => bill.BillAmount));
		}
	}
}
