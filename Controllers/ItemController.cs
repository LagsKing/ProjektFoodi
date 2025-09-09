using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodieApp.Api.Data;
using FoodieApp.Api.Dtos;
using FoodieApp.Api.Models;

namespace FoodieApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ItemsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: api/items?categoryId=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            var query = _db.Items
                .Include(i => i.Category)
                .Include(i => i.Reviews)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(i => i.CategoryId == categoryId.Value);

            var items = await query
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    PhotoUrl = i.PhotoUrl,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category.Name,
                    ReviewsCount = i.Reviews.Count,
                    AverageRating = i.Reviews.Any() ? i.Reviews.Average(r => r.Rating) : null
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Items
                .Include(i => i.Category)
                .Include(i => i.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();

            var dto = new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                PhotoUrl = item.PhotoUrl,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                ReviewsCount = item.Reviews.Count,
                AverageRating = item.Reviews.Any() ? item.Reviews.Average(r => r.Rating) : null
            };

            return Ok(dto);
        }

        // POST api/items
        [HttpPost]
        [RequestSizeLimit(10_000_000)] // np. max 10MB
        public async Task<IActionResult> Create([FromForm] ItemCreateDto dto)
        {
            string? photoUrl = null;

            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using (var stream = System.IO.File.Create(filePath))
                {
                    await dto.Photo.CopyToAsync(stream);
                }

                // ścieżka dostępna z frontu
                photoUrl = $"/uploads/{fileName}";
            }

            var item = new Item
            {
                Name = dto.Name,
                Price = dto.Price,
                PhotoUrl = photoUrl,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                CategoryId = dto.CategoryId,
                CreatedById = dto.CreatedById
            };

            _db.Items.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, new { item.Id });
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemCreateDto dto) // możesz zrobić osobny UpdateDto
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null) return NotFound();

            item.Name = dto.Name;
            item.Price = dto.Price;
            item.Latitude = dto.Latitude;
            item.Longitude = dto.Longitude;
            item.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null) return NotFound();

            // usuń plik zdjęcia (jeśli istnieje)
            if (!string.IsNullOrEmpty(item.PhotoUrl))
            {
                var filePath = Path.Combine(_env.WebRootPath ?? "wwwroot", item.PhotoUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}