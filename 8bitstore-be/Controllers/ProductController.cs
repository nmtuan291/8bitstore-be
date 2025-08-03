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
            if (request == null)
            {
                return BadRequest("Request is empty");
            }

            var products = await _productService.GetProductsAsync(request);
            return Ok(products);          
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductRequest request)
        {
            var products = await _productService.GetAllProductAsync();
            return Ok(products);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct([FromQuery] ProductRequest request)
        {
            if (request.ProductId == null)
            {
                return BadRequest("Product Id is missing.");
            }

            try
            {
                var product = await _productService.GetProductAsync(request.ProductId);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return Ok("Add product successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-suggestion")]
        public async Task<IActionResult> GetSuggestion([FromQuery] string query)
        {
            try
            {
                IEnumerable<string> productNames = await _productService.GetSuggestionAsync(query);
                return Ok(productNames);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
