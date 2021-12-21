namespace DotNetBanky.Common.ConfigModels
{
    public class DatabaseConfiguration
    {
        public bool UseInMemoryDatabase { get; set; } = false;

        public string ConnectionString { get; set; } = null!;
    }
}
