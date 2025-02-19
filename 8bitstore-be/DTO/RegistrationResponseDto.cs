namespace _8bitstore_be.DTO
{
    public class RegistrationResponseDto
    {
        public bool isSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
