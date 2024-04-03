 namespace ShoppingWebsite.Models
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string ImageUrl { get; set; }
            public int StockQuantity { get; set; }
            public bool IsAvailable => StockQuantity > 0;
            // Add other properties as needed
        }
    }

