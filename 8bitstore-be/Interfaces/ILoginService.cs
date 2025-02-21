using _8bitstore_be.DTO;

namespace _8bitstore_be.Interfaces
{
    public interface ILoginService
    {
        public Task<AuthResponseDto> LoginAsync(UserLoginDto user);
    }
}
