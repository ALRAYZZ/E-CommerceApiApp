﻿namespace E_CommerceShared.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
