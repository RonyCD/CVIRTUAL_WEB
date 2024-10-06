using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Domain.Contract
{
    public interface ISeguridadDbContext
    {
        string SQLCnn();

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

        void Dispose();

        EntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        void SaveChanges(string jsonAuthN);

        Task SaveChangesAsync(string jsonAuthN);

        void SaveAudit();

        DbSet<T> Set<T>() where T : class;
    }
}
