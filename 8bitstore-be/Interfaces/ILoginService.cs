using _8bitstore_be.DTO.User;

namespace _8bitstore_be.Interfaces
{
    public interface ILoginService
    {
        public Task<AuthResponseDto> LoginAsync(UserLoginDto user);
        public Task<UserDto> GetUserAsync(string username);
        public Task LogoutAsync();
    }
}
