namespace DotNetBanky.Core.Constants
{
    public static class TransactionConstants
    {
        public const string Withdraw = "Withdraw";
        public const string Deposit = "Deposit";
        public const string Transaction = "Transaction";

        public static readonly List<string> TransactionOperations = new() { Withdraw, Deposit, Transaction };

        public const string Credit = "Credit";
        public const string Debit = "Debit";

        public static readonly List<string> TransactionTypes = new() { Credit, Debit };
    }
}
