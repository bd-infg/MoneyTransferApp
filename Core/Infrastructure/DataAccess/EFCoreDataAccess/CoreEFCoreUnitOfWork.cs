using Common.EFCoreDataAccess;
using Domain.Repositories;
using EFCoreDataAccess.Repositories;

namespace EFCoreDataAccess
{
    public class CoreEFCoreUnitOfWork : EFCoreUnitOfWork, ICoreUnitOfWork
    {

        public IAccountRepository AccountRepository { get; }

        public ITransactionRepository TransactionRepository { get; }
        public ISystemParameterRepository SystemParameterRepository { get; }
        

        public CoreEFCoreUnitOfWork(CoreEFCoreDbContext context) : base(context)
        {
            AccountRepository = new AccountRepository(context);
            TransactionRepository = new TransactionRepository(context);
            SystemParameterRepository = new SystemParameterRepository(context);
        }
    }
}
