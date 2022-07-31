namespace QvaCar.Infraestructure.Chat.Configuration
{
    internal class ChatDbSettings
    {
        public const string SectionName = "ChatDb";
        public string DatabaseConnectionString { get; set; } = string.Empty;
    }
}
