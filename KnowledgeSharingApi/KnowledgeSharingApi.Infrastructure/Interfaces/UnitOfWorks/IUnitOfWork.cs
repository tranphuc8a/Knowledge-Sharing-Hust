using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        IUnitOfWork RegisterRepository<T>(IRepository<T> repository) where T : Entity;

        IUnitOfWork RegisterRepository<T>(IEnumerable<IRepository<T>> repositories) where T : Entity;
    }
}
