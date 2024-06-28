using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _DbContext;
        private readonly IResourceFactory _ResourceFactory;
        private IDbTransaction? Transaction;
        public UnitOfWork(IDbContext dbContext, IResourceFactory resourceFactory)
        {
            _DbContext = dbContext;
            _ResourceFactory = resourceFactory;
        }

        public virtual void BeginTransaction()
        {
            if (_DbContext.Connection.State != ConnectionState.Open)
            {
                _DbContext.Connection.Open();
            }
            if (Transaction != null)
            {
                throw new NullReferenceException(_ResourceFactory.GetResponseResource().TransactionNotOpen());
            }
            Transaction = _DbContext.Connection.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            IDbTransaction? transaction = Transaction;
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException(_ResourceFactory.GetResponseResource().TransactionNotClose());
            }
        }

        public virtual void Dispose()
        {
            Transaction?.Dispose();
            if (_DbContext.Connection.State == ConnectionState.Open)
            {
                _DbContext.Connection.Close();
            }
            Transaction = null;
        }

        public virtual void RollbackTransaction()
        {
            IDbTransaction? transaction = Transaction;
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException(_ResourceFactory.GetResponseResource().TransactionNotClose());
            }
        }

        public virtual IUnitOfWork RegisterRepository<T>(IRepository<T> repository) where T : Entity
        {
            if (Transaction == null)
            {
                throw new NullReferenceException(_ResourceFactory.GetResponseResource().TransactionNotOpen());
            }
            repository.RegisterTransaction(Transaction);
            return this;
        }

        public virtual IUnitOfWork RegisterRepository<T>(List<IRepository<T>> repositories) where T : Entity
        {
            if (Transaction == null)
            {
                throw new NullReferenceException(_ResourceFactory.GetResponseResource().TransactionNotOpen());
            }
            foreach (IRepository<T> repo in repositories)
            {
                repo.RegisterTransaction(Transaction);
            }
            return this;
        }

    }
}
