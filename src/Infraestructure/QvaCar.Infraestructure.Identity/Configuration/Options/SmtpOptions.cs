namespace QvaCar.Infraestructure.Identity.Configuration
{
    public class SmtpOptions
    {
        public const string Section = "Smtp";
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
