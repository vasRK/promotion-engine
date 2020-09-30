namespace PromotionEngine.Models
{
	public class Product
	{
		public int Id { get; set; }

		public string SKU { get; set; }

		public double Price { get; set; }

		public override string ToString()
		{
			return this.SKU;
		}
	}
}
