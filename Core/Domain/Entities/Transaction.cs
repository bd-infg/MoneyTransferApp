namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; private set; }
        public decimal Amount { get; private set; }
        public string FromAccountId { get; private set; }
        public string ToAccountId { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionFlowType Flow { get; private set; }

        public Transaction()
        {

        }

        public Transaction(decimal amount, string fromAccount, string toAccount, TransactionType type, TransactionFlowType flow)
        {
            Amount = amount;
            FromAccountId = fromAccount;
            ToAccountId = toAccount;
            Type = type;
            Flow = flow;
        }
    }
}
