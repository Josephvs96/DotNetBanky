namespace DotNetBanky.Core.Constants
{
    public static class FrequencyConstants
    {
        public const string Monthly = "Monthly";
        public const string Weekly = "Weekly";
        public const string AfterTransaction = "AfterTransaction";

        public static List<string> FrequencyList = new List<string>()
        {
            Monthly, Weekly, AfterTransaction
        };
    }
}
