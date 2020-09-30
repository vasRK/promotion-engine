using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine.Models.Service
{
	public interface IPromotionService
	{
		void ApplyPromotion(List<CartProduct> cartProducts);
	}
}
