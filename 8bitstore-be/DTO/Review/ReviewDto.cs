using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Review
{
    public class ReviewDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string Comment { get; set; }

        public string? UserName { get; set; }

        public string? ImgUrl {  get; set; }
        public DateTime? ReviewDate { get; set; }
    }
}
