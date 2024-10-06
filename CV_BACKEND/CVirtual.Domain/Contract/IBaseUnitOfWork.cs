using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Contract
{
    public interface IBaseUnitOfWork
    {
        IContext Context { get; set; }

        int Save();

        Task<int> SaveAsync();

        int Save(string jsonAuthN);

        Task<int> SaveAsync(string jsonAuthN);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        bool Commit();

        void Rollback();
    }
}
