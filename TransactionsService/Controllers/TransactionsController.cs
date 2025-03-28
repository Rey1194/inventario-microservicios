using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TransactionsService.Data;
using TransactionsService.Models;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly TransactionDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _productsServiceUrl = "http://localhost:5001/api/products"; // URL de ProductsService

    public TransactionsController(TransactionDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        return await _context.Transactions.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        // Obtener información del producto
        var response = await _httpClient.GetAsync($"{_productsServiceUrl}/{transaction.ProductId}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound("Producto no encontrado.");
        }

        var productJson = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<ProductDto>(productJson);

        if (transaction.Type.ToLower() == "venta" && product.Stock < transaction.Quantity)
        {
            return BadRequest("Stock insuficiente para la venta.");
        }

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        // Actualizar stock en ProductsService
        var stockUpdate = new { ProductId = transaction.ProductId, Quantity = transaction.Quantity, Type = transaction.Type };
        var content = new StringContent(JsonConvert.SerializeObject(stockUpdate), Encoding.UTF8, "application/json");

        await _httpClient.PutAsync($"{_productsServiceUrl}/update-stock", content);

        return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
    }
}

// DTO para recibir información del producto
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
}
