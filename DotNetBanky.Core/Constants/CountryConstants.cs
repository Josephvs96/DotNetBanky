namespace DotNetBanky.Core.Constants
{
    public static class CountryConstants
    {
        public const string Sweden = "Sweden";
        public const string Denmark = "Denmark";
        public const string Finland = "Finland";
        public const string Norway = "Norway";

        public static List<string> CountryList = new() { Denmark, Finland, Norway, Sweden };

        public static Dictionary<string, string> CountryCodes = new()
        {
            { Sweden, "SE" },
            { Denmark, "DK" },
            { Finland, "FI" },
            { Norway, "NO" }
        };
    }
}
