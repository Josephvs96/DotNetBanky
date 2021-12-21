namespace DotNetBanky.Core.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
