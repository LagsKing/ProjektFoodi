namespace FoodieApp.Api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } 
        public string? Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}