using Dapper;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories
{
    public class AdministrationRepository : IAdministrationRepository
    {
        public IDbContext DbContext { get; set; }
        public IDbConnection Connection { get; set; }
        public IDbTransaction? Transaction { get; set; }

        public AdministrationRepository(IDbContext dbContext)
        {
            if (dbContext is not MySqlDbContext)
            {
                throw new DbContextNotMatchTypeException();
            }
            DbContext = dbContext;
            Connection = dbContext.Connection;
        }

        public async Task<object> QueryByConnection(string query)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                var result = await Connection.QueryAsync(query, transaction: Transaction);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }

        public async Task<object> QueryByDbContext(string query)
        {
            try
            {
                if (DbContext.Connection.State != ConnectionState.Open)
                {
                    DbContext.Connection.Open();
                }

                var result = await DbContext.Connection.QueryAsync(query);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DbContext.Connection.Close();
            }
        }
    }
}
