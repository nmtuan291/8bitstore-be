using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null)
            {
                return BadRequest("Request is empty");
            }

            var products = await _productService.GetProductsAsync(request.SortByName, request.SortByPrice, request.SortByDate);

            return Ok(products);          
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct([FromQuery] ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.ProductId == null)
            {
                return BadRequest("Product Id is missing.");
            }
            
            var product = await _productService.GetProductAsync(request.ProductId);

            if (product == null)
            {
                return NotFound("Cannot find product.");
            }

            return Ok(product);
        }
    }
}
