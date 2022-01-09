namespace DotNetBanky.Core.Exceptions
{
    public class AccountNotOwnedByCustomerException : Exception
    {
        public AccountNotOwnedByCustomerException() : base("Account not owned by the customer") { }
    }

    public class TransactionAmountLargerThanBalanceException : Exception
    {
        public TransactionAmountLargerThanBalanceException() : base("The amount of the transaction is larger than the balance of the account") { }
    }

    public class AccountToNotProvided : Exception
    {
        public AccountToNotProvided() : base("The To-Account is not provided")
        {

        }
    }
}
