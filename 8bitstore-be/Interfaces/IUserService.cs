namespace _8bitstore_be.Interfaces
{
    public interface IUserService
    {
        public Task ChangeAddressAsync(string userId, string address, string city, string district, string subDistrict);
        public Task ChangePasswordAsync(string userId, string newPassword, string currentPassword);
    }
}
