namespace QvaCar.Infraestructure.BlogStorage.Configuration.Options
{
    public class ImageServiceOptions
    {
        public const string Section = "AzureBlogStorage";
        public string BlobStorageConnectionString { get; init; }
        public string ContainerName { get; init; }
        public string BaseUrl { get; init; }
    }
}
