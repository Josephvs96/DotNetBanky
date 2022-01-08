namespace DotNetBanky.Core.Constants
{
    public static class DispostionsConstants
    {
        public const string Owner = "OWNER";
        public const string Disponent = "DISPONENT";

        public static readonly Dictionary<string, string> DispotionsList = new Dictionary<string, string>
        {
            {"Owner", Owner },
            {"Disponent", Disponent }
        };
    }
}
