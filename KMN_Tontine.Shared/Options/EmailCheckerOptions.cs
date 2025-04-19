namespace KMN_Tontine.Shared.Options
{
    public class EmailCheckerOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string EmailFolder { get; set; } = "INBOX";
    }
} 