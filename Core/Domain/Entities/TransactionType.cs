namespace Domain.Entities
{
    public enum TransactionType : byte
    {
        BankWithdrawalToWallet = 1,
        BankDepositToWallet = 2,
        IntraWallet = 3,
        Compensation = 4
    }
}
