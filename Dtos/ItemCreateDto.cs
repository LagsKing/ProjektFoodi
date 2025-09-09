using Microsoft.AspNetCore.Http;

namespace FoodieApp.Api.Dtos
{
    public class ItemCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public IFormFile? Photo { get; set; }   
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int CategoryId { get; set; }
        public int? CreatedById { get; set; }
    }
}