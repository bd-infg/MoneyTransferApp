using Utils;

namespace Domain.Repositories
{
    public interface ICoreUnitOfWork : IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        ISystemParameterRepository SystemParameterRepository { get; }

    }
}
