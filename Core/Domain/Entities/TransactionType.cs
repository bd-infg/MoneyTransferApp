namespace Domain.Entities
{
    public enum TransactionType : byte
    {
        BankWithdrawalToWallet = 1,
        BankDepositFromWallet = 2,
        IntraWallet = 3,
        Compensation = 4
    }
}
