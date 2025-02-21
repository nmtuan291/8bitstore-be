namespace _8bitstore_be.DTO
{
    public class AuthResponseDto
    {
        public bool isSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public UserDto? User { get; set; }
    }
}
