using CVirtual.Domain.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.DataAccess.SQLServer.Context
{
    public class SeguridadDbContext : DbContext, ISeguridadDbContext
    {
        public readonly string _connstr;

        public SeguridadDbContext(string connstr)
        {
            this._connstr = connstr;
        }

        public SeguridadDbContext(DbContextOptions<SeguridadDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(this._connstr))
            {
                optionsBuilder.UseSqlServer(this._connstr);
            }
        }

        public string SQLCnn()
        {
            return this._connstr;
        }

        public void SaveChanges(string jsonAuthN)
        {
            //TODO
        }

        public async Task SaveChangesAsync(string jsonAuthN)
        {
            //TODO
            await Task.Delay(0);
        }

        public void SaveAudit()
        {
            //TODO
        }
    }
}
