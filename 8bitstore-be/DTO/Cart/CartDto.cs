using System.Collections.Generic;

namespace _8bitstore_be.DTO.Cart
{
    public class CartDto
    {
        public CartDto()
        {
            CartItems = new List<CartItemDto>();
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
