namespace FoodieApp.Api.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? PhotoUrl { get; set; }    
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}