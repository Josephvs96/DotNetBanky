namespace DotNetBanky.Core.DTOModels.Fraud
{
    public class FraudDTO
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int TransactionId { get; set; }
    }
}