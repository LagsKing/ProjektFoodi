namespace FoodieApp.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}