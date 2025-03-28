using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;

    public ProductsController(ProductDbContext context)
    {
        _context = context;
    }

    // ✅ Obtener todos los productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // ✅ Obtener un producto por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Producto no encontrado.");
        }
        return product;
    }

    // ✅ Crear un nuevo producto
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (product.Stock < 0)
        {
            return BadRequest("El stock no puede ser negativo.");
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // ✅ Actualizar un producto existente
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("El ID del producto no coincide.");
        }

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound("Producto no encontrado.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Category = product.Category;
        existingProduct.ImageUrl = product.ImageUrl;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ✅ Eliminar un producto
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Producto no encontrado.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ✅ Endpoint para actualizar el stock (compra/venta)
    [HttpPut("update-stock")]
    public async Task<IActionResult> UpdateStock([FromBody] StockUpdateDto stockUpdate)
    {
        var product = await _context.Products.FindAsync(stockUpdate.ProductId);
        if (product == null)
        {
            return NotFound("Producto no encontrado.");
        }

        if (stockUpdate.Type.ToLower() == "venta")
        {
            if (product.Stock < stockUpdate.Quantity)
            {
                return BadRequest("Stock insuficiente.");
            }
            product.Stock -= stockUpdate.Quantity;
        }
        else if (stockUpdate.Type.ToLower() == "compra")
        {
            product.Stock += stockUpdate.Quantity;
        }
        else
        {
            return BadRequest("Tipo de transacción no válido. Usa 'compra' o 'venta'.");
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Stock actualizado correctamente.", newStock = product.Stock });
    }
}

// DTO para actualizar stock
public class StockUpdateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; } // "compra" o "venta"
}
