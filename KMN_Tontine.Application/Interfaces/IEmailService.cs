public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string htmlContent);
    Task<bool> SendEmailConfirmationAsync(string to, string userName, string confirmationLink);
    Task<bool> SendAccountApprovedAsync(string to, string userName, string confirmationLink);
    Task<bool> SendAccountRejectedAsync(string to, string userName, string reason);
} 