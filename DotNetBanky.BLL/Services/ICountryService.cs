namespace DotNetBanky.BLL.Services
{
    public interface ICountryService
    {
        Task<string> GetCountryFlagUrl(string countryName);
    }
}
