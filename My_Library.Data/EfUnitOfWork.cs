using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using Castle.Core.Logging;
using My_Library.Core.Data;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace My_Library.Data
{
    public class EfUnitOfWork
         : IUnitOfWork
    {
        public DbContext Context { get; set; }


        public ILogger Logger { get; set; }

        public EfUnitOfWork(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            Context = context;
        }

        public void Commit()
        {
            if (Context != null)
            {
                try
                {
                    // SaveChanges also uses transaction which uses by default ReadCommitted isolation
                    // level but TransactionScope uses by default more restrictive Serializable isolation
                    // level 
                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                                            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        Context.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.CreateChildLogger("EfUnitOfWork").Error("Unable to save changes", ex);
                }
                catch (OptimisticConcurrencyException ex)
                {
                    Logger.CreateChildLogger("EfUnitOfWork").Error("Unable to save changes", ex);
                    //var entry = ex.StateEntries[0];
                    ////Context.Refresh(RefreshMode.StoreWins, entry.Entity);
                    //Context.SaveChanges();
                    // Logger.Instance.Error("System.Entity.Framework", "", "", entry.EntityKey.ToString());
                }
                catch (DataException ex)
                {
                    Logger.CreateChildLogger("EfUnitOfWork").Error("Unable to save changes", ex);
                }
            }
        }

        public void Rollback()
        {
            // not mented
        }

        #region IDisposable Members

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                        Context = null;
                    }

                }

                // Clean up any unmanged resources here.
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //~EfUnitOfWork()
        //{
        //    Dispose(false);
        //}

        #endregion
    }
}
