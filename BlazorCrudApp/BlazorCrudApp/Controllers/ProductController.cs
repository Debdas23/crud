using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorCrudApp.Data;
using BlazorCrudApp.Shared.Models;

namespace BlazorCrudApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
        Console.WriteLine("🔧 ProductController initialized.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get()
    {
        try
        {
            Console.WriteLine("✅ [GET] /api/product called");
            var products = await _context.Products.ToListAsync();
            Console.WriteLine($"📦 Returning {products.Count} products");
            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ GET error: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Product product)
    {
        try
        {
            Console.WriteLine($"📥 [POST] Adding product: {product.Name}, ₹{product.Price}");
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Product saved.");
            return Ok(product);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ POST error: " + ex.Message);
            return StatusCode(500, "Failed to save product");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Product product)
    {
        try
        {
            Console.WriteLine($"📝 [PUT] Updating product ID: {id}");
            var existing = await _context.Products.FindAsync(id);
            if (existing is null)
            {
                Console.WriteLine("⚠️ Product not found");
                return NotFound();
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Product updated.");
            return Ok(existing);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ PUT error: " + ex.Message);
            return StatusCode(500, "Failed to update product");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            Console.WriteLine($"🗑️ [DELETE] Product ID: {id}");
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                Console.WriteLine("⚠️ Product not found");
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Product deleted.");
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ DELETE error: " + ex.Message);
            return StatusCode(500, "Failed to delete product");
        }
    }
}
