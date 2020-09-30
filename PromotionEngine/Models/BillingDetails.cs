using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine.Models
{
	public class BillingDetails
	{
		public string Statement { get; set; }

		public double BillAmount { get; set; }

		public Product Product { get; set; }

		public int Count { get; set; }
	}
}
