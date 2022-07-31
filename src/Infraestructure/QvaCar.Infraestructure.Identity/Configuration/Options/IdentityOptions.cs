namespace QvaCar.Infraestructure.Identity.Configuration
{
    public class IdentityOptions
    {
        public const string SectionName = "Identity";
        public string DatabaseConnectionString { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
