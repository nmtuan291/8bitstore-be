using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.DTO
{
    public class StatusResponse<T>
    {
        [Required]
        public string Status {  get; set; }
        public T Message { get; set; }
    }
}
