namespace FoodieApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; } 
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}