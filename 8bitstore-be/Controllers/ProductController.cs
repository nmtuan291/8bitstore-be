using _8bitstore_be.DTO.Product;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) 
        {
            _productService = productService;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductRequest request)
        {
            var products = await _productService.GetProductsAsync(request);
            return Ok(products);          
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _productService.GetAllProductAsync();
            return Ok(products);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct([FromQuery] ProductRequest request)
        {
            if (request.ProductId == null)
                return BadRequest(new { error = "Product Id is missing." });

            var product = await _productService.GetProductAsync(request.ProductId);
            return Ok(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
           await _productService.AddProductAsync(product);
           return Ok();
        }

        [HttpGet("get-suggestion")]
        public async Task<IActionResult> GetSuggestion([FromQuery] string query)
        {
            IEnumerable<string> productNames = await _productService.GetSuggestionAsync(query);
            return Ok(productNames);
        }

    }
}
