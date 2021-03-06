using Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Common.EFCoreDataAccess
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly EFCoreDbContext Context;
        private IDbContextTransaction Transaction;

        public EFCoreUnitOfWork(EFCoreDbContext context)
        {
            Context = context;
        }

        public async Task BeginTransactionAsync()
        {
            Transaction = await Context.Database.BeginTransactionAsync();
        }
        public Task CommitTransactionAsync()
        {
            return Transaction.CommitAsync();
        }
        public Task RollbackTransactionAsync()
        {
            return Transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duce)
            {
                //todo: log
                throw duce;
            }
            catch (DbUpdateException due)
            {
                throw due;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region IDisposable implementation

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {

            if (disposedValue)
            {
                return;
            }
            if (disposing)
            {
                Transaction?.Dispose();
                Context.Dispose();
            }
            disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion IDisposable implementation
    }
}
