namespace _8bitstore_be.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string body, string subject);
    }
}
