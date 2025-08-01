namespace ShopMigrationAPI.Models.DTOs;

public class OrderRequestDTO
{
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public List<Cart> Items { get; set; }
}