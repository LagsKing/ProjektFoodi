namespace FoodieApp.Api.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? PhotoUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int ReviewsCount { get; set; }
        public double? AverageRating { get; set; }
    }
}