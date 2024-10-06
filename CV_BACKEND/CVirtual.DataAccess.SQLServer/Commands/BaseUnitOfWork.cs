using CVirtual.Domain.Contract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilsParameter = CVirtual.DataAccess.SQLServer.Utils.Parameter;


namespace CVirtual.DataAccess.SQLServer.Commands
{
    public class BaseUnitOfWork : IBaseUnitOfWork, IDisposable
    {
        public static Dictionary<string, string> Operators = new Dictionary<string, string>()
    {
      {
        "SELECT ",
        "--SELECT--"
      },
      {
        "WHERE ",
        "--WHERE--"
      },
      {
        "DROP ",
        "--DROP--"
      },
      {
        "EXECUTE ",
        "--EXECUTE--"
      },
      {
        "EXEC ",
        "--EXEC--"
      },
      {
        "dbo.",
        "--dbo.--"
      },
      {
        "sys.",
        "--sys.--"
      },
      {
        "DELETE ",
        "--DELETE--"
      },
      {
        "TRUNCATE ",
        "--TRUNCATE--"
      },
      {
        "FROM ",
        "--FROM--"
      },
      {
        "HAVING ",
        "--HAVING--"
      },
      {
        "INSERT ",
        "--INSERT--"
      },
      {
        "UPDATE ",
        "--UPDATE--"
      },
      {
        "CREATE ",
        "--CREATE--"
      },
      {
        "GRANT ",
        "--GRANT--"
      }
    };
        private readonly bool _clearInjection;
        public ISeguridadDbContext _ctx;
        private bool _disposed;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public BaseUnitOfWork(ISeguridadDbContext ctx, bool clearInjection = true)
        {
            this._ctx = ctx;
            this._disposed = false;
            this._clearInjection = clearInjection;
        }

        public IContext Context { get; set; }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).State != ConnectionState.Open)
                RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).Open();
            this._transaction = RelationalDatabaseFacadeExtensions.BeginTransaction(this._ctx.Database, isolationLevel);
        }

        public bool Commit()
        {
            IDbContextTransaction transaction = this._transaction;
            if ((transaction != null ? DbContextTransactionExtensions.GetDbTransaction(transaction) : (DbTransaction)null) == null)
                return false;
            this._transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            if (DbContextTransactionExtensions.GetDbTransaction(this._transaction) == null)
                return;
            this._transaction.Rollback();
        }

        public int Save()
        {
            bool flag;
            do
            {
                try
                {
                    return this._ctx.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    flag = true;
                    EntityEntry entityEntry = ((IEnumerable<EntityEntry>)((DbUpdateException)ex).Entries).Single<EntityEntry>();
                    entityEntry.OriginalValues.SetValues(entityEntry.GetDatabaseValues());
                    //entityEntry.get_OriginalValues().SetValues(entityEntry.GetDatabaseValues());
                }
                catch (DbUpdateException ex)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("DbUpdateException error details - " + ((Exception)ex).Message);
                    stringBuilder.AppendLine(((Exception)ex)?.InnerException?.Message);
                    using (IEnumerator<EntityEntry> enumerator = ((IEnumerable<EntityEntry>)ex.Entries).GetEnumerator())
                    {
                        while (((IEnumerator)enumerator).MoveNext())
                        {
                            EntityEntry current = enumerator.Current;
                            stringBuilder.AppendLine(string.Format("Entity of type {0} in state {1} could not be updated", (object)current.Entity.GetType().Name, (object)current.State));
                        }
                    }
                    throw new Exception(stringBuilder.ToString(), (Exception)ex);
                }
                catch
                {
                    throw;
                }
            }
            while (flag);
            return 0;
        }

        public int Save(string jsonAuthN)
        {
            this._ctx.SaveChanges(jsonAuthN);
            int num = this.Save();
            if (num <= 0)
                return num;
            this.AsyncSaveAudit();
            return num;
        }

        public async Task<int> SaveAsync()
        {
            bool flag;
            do
            {
                try
                {
                    return await this._ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    flag = true;
                    EntityEntry entityEntry = ((IEnumerable<EntityEntry>)((DbUpdateException)ex).Entries).Single<EntityEntry>();
                    entityEntry.OriginalValues.SetValues(entityEntry.GetDatabaseValues());
                }
                catch (DbUpdateException ex)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("DbUpdateException error details - " + ((Exception)ex).Message);
                    stringBuilder.AppendLine(((Exception)ex)?.InnerException?.Message);
                    using (IEnumerator<EntityEntry> enumerator = ((IEnumerable<EntityEntry>)ex.Entries).GetEnumerator())
                    {
                        while (((IEnumerator)enumerator).MoveNext())
                        {
                            EntityEntry current = enumerator.Current;
                            stringBuilder.AppendLine(string.Format("Entity of type {0} in state {1} could not be updated", (object)current.Entity.GetType().Name, (object)current.State));
                        }
                    }
                    throw new Exception(stringBuilder.ToString(), (Exception)ex);
                }
                catch
                {
                    throw;
                }
            }
            while (flag);
            return 0;
        }

        public async Task<int> SaveAsync(string jsonAuthN)
        {
            await this._ctx.SaveChangesAsync(jsonAuthN);
            int num = await this.SaveAsync();
            if (num > 0)
                this.AsyncSaveAudit();
            return num;
        }

        protected IDbConnection Connection => (IDbConnection)RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database);

        protected IDbTransaction Transaction
        {
            get
            {
                IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                return currentTransaction == null ? (IDbTransaction)null : (IDbTransaction)DbContextTransactionExtensions.GetDbTransaction(currentTransaction);
            }
        }

        protected int ExecuteNonQuery(string sqlText, ref UtilsParameter[] parameters, int? commandTimeout = null) => this.ExecuteNonQuery(sqlText, CommandType.StoredProcedure, ref parameters, commandTimeout);

        protected int ExecuteNonQuery(
          string sqlText,
          CommandType commandType,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            int num;
            using (DbCommand command = RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).CreateCommand())
            {
                if (command.Connection.State != ConnectionState.Open)
                    RelationalDatabaseFacadeExtensions.OpenConnection(this._ctx.Database);
                if (this._transaction != null)
                {
                    DbCommand dbCommand = command;
                    IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                    DbTransaction dbTransaction = currentTransaction != null ? DbContextTransactionExtensions.GetDbTransaction(currentTransaction) : (DbTransaction)null;
                    dbCommand.Transaction = dbTransaction;
                }
                command.Parameters.Clear();
                command.CommandText = sqlText;
                command.CommandType = commandType;
                command.CommandTimeout = commandTimeout ?? command.CommandTimeout;
                if (parameters != null && parameters.Length != 0)
                {
                    foreach (UtilsParameter parameter1 in parameters)
                    {
                        IDbDataParameter parameter2 = (IDbDataParameter)command.CreateParameter();
                        parameter2.ParameterName = parameter1.Name;
                        parameter2.Value = this.CustomValue(parameter1.Value);
                        parameter2.Direction = parameter1.Direction;
                        parameter2.Size = parameter1.Size;
                        command.Parameters.Add((object)parameter2);
                    }
                }
                num = command.ExecuteNonQuery();
                parameters = this.OutParameters(command.Parameters, parameters);
                command.Parameters.Clear();
            }
            return num;
        }

        protected IEnumerable<T> ExecuteReader<T>(
          string sqlText,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            return this.ExecuteReader<T>(sqlText, CommandType.StoredProcedure, ref parameters, commandTimeout);
        }

        protected IEnumerable<T> ExecuteReader<T>(
          string sqlText,
          CommandType commandType,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            List<T> objList = new List<T>();
            using (DbCommand command = RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).CreateCommand())
            {
                if (command.Connection.State != ConnectionState.Open)
                    RelationalDatabaseFacadeExtensions.OpenConnection(this._ctx.Database);
                if (this._transaction != null)
                {
                    DbCommand dbCommand = command;
                    IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                    DbTransaction dbTransaction = currentTransaction != null ? DbContextTransactionExtensions.GetDbTransaction(currentTransaction) : (DbTransaction)null;
                    dbCommand.Transaction = dbTransaction;
                }
                command.Parameters.Clear();
                command.CommandText = sqlText;
                command.CommandType = commandType;
                command.CommandTimeout = commandTimeout ?? command.CommandTimeout;
                if (parameters != null && parameters.Length != 0)
                {
                    foreach (UtilsParameter parameter1 in parameters)
                    {
                        IDbDataParameter parameter2 = (IDbDataParameter)command.CreateParameter();
                        parameter2.ParameterName = parameter1.Name;

                        parameter2.Value = this.CustomValue(parameter1.Value);
                        parameter2.Direction = parameter1.Direction;
                        parameter2.Size = parameter1.Size;
                        command.Parameters.Add((object)parameter2);
                    }
                }
                using (DbDataReader dbDataReader = command.ExecuteReader())
                {
                    if (dbDataReader.HasRows)
                    {
                        int fieldCount = dbDataReader.FieldCount;
                        Dictionary<int, string> dictionary = new Dictionary<int, string>();
                        for (int index = 0; index < fieldCount; ++index)
                            dictionary.Add(index, dbDataReader.GetName(index));
                        object[] values = new object[fieldCount];
                        while (dbDataReader.Read())
                        {
                            dbDataReader.GetValues(values);
                            T instance = Activator.CreateInstance<T>();
                            foreach (PropertyInfo runtimeProperty in instance.GetType().GetRuntimeProperties())
                            {
                                foreach (KeyValuePair<int, string> keyValuePair in dictionary)
                                {
                                    if (!(runtimeProperty.Name.ToUpper() != keyValuePair.Value.ToUpper()))
                                    {
                                        object obj = values[keyValuePair.Key];
                                        if (obj.GetType() != typeof(DBNull))
                                            runtimeProperty.SetValue((object)instance, obj, (object[])null);
                                    }
                                }
                            }
                            objList.Add(instance);
                        }
                    }
                }
                parameters = this.OutParameters(command.Parameters, parameters);
                command.Parameters.Clear();
            }
            return (IEnumerable<T>)objList;
        }

        protected T ExecuteScalar<T>(string sqlText, ref UtilsParameter[] parameters, int? commandTimeout = null) => this.ExecuteScalar<T>(sqlText, CommandType.StoredProcedure, ref parameters, commandTimeout);

        protected T ExecuteScalar<T>(
          string sqlText,
          CommandType commandType,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            T obj;
            using (DbCommand command = RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).CreateCommand())
            {
                if (command.Connection.State != ConnectionState.Open)
                    RelationalDatabaseFacadeExtensions.OpenConnection(this._ctx.Database);
                if (this._transaction != null)
                {
                    DbCommand dbCommand = command;
                    IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                    DbTransaction dbTransaction = currentTransaction != null ? DbContextTransactionExtensions.GetDbTransaction(currentTransaction) : (DbTransaction)null;
                    dbCommand.Transaction = dbTransaction;
                }
                command.Parameters.Clear();
                command.CommandText = sqlText;
                command.CommandType = commandType;
                command.CommandTimeout = commandTimeout ?? command.CommandTimeout;
                if (parameters != null && parameters.Length != 0)
                {
                    foreach (UtilsParameter parameter1 in parameters)
                    {
                        IDbDataParameter parameter2 = (IDbDataParameter)command.CreateParameter();
                        parameter2.ParameterName = parameter1.Name;
                        parameter2.Value = this.CustomValue(parameter1.Value);
                        parameter2.Direction = parameter1.Direction;
                        parameter2.Size = parameter1.Size;
                        command.Parameters.Add((object)parameter2);
                    }
                }
                obj = (T)command.ExecuteScalar();
                parameters = this.OutParameters(command.Parameters, parameters);
                command.Parameters.Clear();
            }
            return obj;
        }

        /* protected int ExecuteSqlCommand(
           string sqlText,
           Parameter[] onlyInputParameters,
           int? commandTimeout = null)
         {
             if (onlyInputParameters != null)
                 sqlText = string.Format("{0} {1}", (object)sqlText, (object)this.FormatParameters(onlyInputParameters));
             return RelationalDatabaseFacadeExtensions.ExecuteSqlInterpolatedAsync(this._ctx.Database, RawSqlString.op_Implicit(sqlText), Array.Empty<object>());
         }

         protected async Task<int> ExecuteSqlCommandAsync(
           string sqlText,
           Parameter[] onlyInputParameters,
           int? commandTimeout = null)
         {
             if (onlyInputParameters != null)
                 sqlText = string.Format("{0} {1}", (object)sqlText, (object)this.FormatParameters(onlyInputParameters));
             return await RelationalDatabaseFacadeExtensions.ExecuteSqlInterpolatedAsync(this._ctx.Database, ExecuteSqlRaw.op_Implicit(sqlText), new CancellationToken());
         }*/

        protected string ExecuteXmlReader(
          string sqlText,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            return this.ExecuteXmlReader(sqlText, CommandType.StoredProcedure, ref parameters, commandTimeout);
        }

        protected string ExecuteXmlReader(
          string sqlText,
          CommandType commandType,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (DbCommand command = RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).CreateCommand())
            {
                if (command.Connection.State != ConnectionState.Open)
                    RelationalDatabaseFacadeExtensions.OpenConnection(this._ctx.Database);
                if (this._transaction != null)
                {
                    DbCommand dbCommand = command;
                    IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                    DbTransaction dbTransaction = currentTransaction != null ? DbContextTransactionExtensions.GetDbTransaction(currentTransaction) : (DbTransaction)null;
                    dbCommand.Transaction = dbTransaction;
                }
                command.Parameters.Clear();
                command.CommandText = sqlText;
                command.CommandType = commandType;
                command.CommandTimeout = commandTimeout ?? command.CommandTimeout;
                if (parameters != null && parameters.Length != 0)
                {
                    foreach (UtilsParameter parameter1 in parameters)
                    {
                        IDbDataParameter parameter2 = (IDbDataParameter)command.CreateParameter();
                        parameter2.ParameterName = parameter1.Name;
                        parameter2.Value = this.CustomValue(parameter1.Value);
                        parameter2.Direction = parameter1.Direction;
                        parameter2.Size = parameter1.Size;
                        command.Parameters.Add((object)parameter2);
                    }
                }
                using (DbDataReader dbDataReader = command.ExecuteReader())
                {
                    if (!dbDataReader.HasRows)
                    {
                        command.Parameters.Clear();
                        return (string)null;
                    }
                    while (dbDataReader.Read())
                    {
                        if (dbDataReader.GetValue(0).GetType() != typeof(DBNull))
                            stringBuilder.Append((string)dbDataReader.GetValue(0));
                    }
                }
                parameters = this.OutParameters(command.Parameters, parameters);
                command.Parameters.Clear();
            }
            return stringBuilder.ToString();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            if (disposing)
            {
                try
                {
                    if (this._ctx != null && RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).State == ConnectionState.Open)
                        RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).Close();
                    if (this.Transaction != null)
                        this.Transaction.Dispose();
                }
                catch (ObjectDisposedException ex)
                {
                }
                this._ctx?.Dispose();
                this._ctx = (ISeguridadDbContext)null;
            }
            this._disposed = true;
        }

        private async void AsyncSaveAudit() => await Task.Run((Action)(() => this._ctx.SaveAudit()));

        private object CustomValue(object value)
        {
            if (value == null)
                return (object)DBNull.Value;
            return this._clearInjection && value is string ? (object)this.MultipleReplace(value.ToString()) : value;
        }

        private string FormatParameters(UtilsParameter[] para)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (UtilsParameter parameter in para)
            {
                if (parameter.Value == null)
                    stringBuilder.Append(string.Format("{0} = {1},", (object)parameter.Name, (object)"null"));
                else if (parameter.Value is string)
                {
                    string text = parameter.Value.ToString();
                    string str = this._clearInjection ? this.MultipleReplace(text) : text;
                    stringBuilder.Append(string.Format("{0} = N'{1}',", (object)parameter.Name, (object)str.Replace("'", "''")));
                }
                else if (parameter.Value is DateTime)
                    stringBuilder.Append(string.Format("{0} = N'{1}',", (object)parameter.Name, (object)Convert.ToDateTime(parameter.Value).ToString("yyyy-MM-dd HH:mm:ss")));
                else if (parameter.Value is int || parameter.Value is long || parameter.Value is short)
                    stringBuilder.Append(string.Format("{0} = {1},", (object)parameter.Name, parameter.Value));
                else if (parameter.Value is bool)
                {
                    bool flag = (bool)parameter.Value;
                    stringBuilder.Append(string.Format("{0} = {1},", (object)parameter.Name, (object)(flag ? 1 : 0)));
                }
                else
                {
                    string str = parameter.Value.ToString().Replace("'", "''");
                    stringBuilder.Append(string.Format("{0} = N'{1}',", (object)parameter.Name, (object)str));
                }
            }
            return stringBuilder.ToString().TrimEnd(',');
        }

        private string MultipleReplace(string text) => BaseUnitOfWork.Operators.Keys.Where<string>(new Func<string, bool>(text.Contains)).Aggregate<string, string>(text, (Func<string, string, string>)((current, textToReplace) => current.Replace(textToReplace, BaseUnitOfWork.Operators[textToReplace])));

        private UtilsParameter[] OutParameters(
          DbParameterCollection cmdParameters,
          UtilsParameter[] parameters)
        {
            if (parameters != null && parameters.Length != 0)
            {
                foreach (object cmdParameter in cmdParameters)
                {
                    IDataParameter pp = (IDataParameter)cmdParameter;
                    if (pp.Direction == ParameterDirection.InputOutput || pp.Direction == ParameterDirection.Output)
                        ((IEnumerable<UtilsParameter>)parameters).First<UtilsParameter>((Func<UtilsParameter, bool>)(x => x.Name == pp.ParameterName)).Value = pp.Value;
                }
                parameters = ((IEnumerable<UtilsParameter>)parameters).Where<UtilsParameter>((Func<UtilsParameter, bool>)(x => x.Direction == ParameterDirection.InputOutput || x.Direction == ParameterDirection.Output)).ToArray<UtilsParameter>();
            }
            return parameters;
        }


        // BEGIN Reader múltiple
        protected DbCommand DbCommandConfig(
          string sqlText,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            return this.DbCommandConfig(sqlText, CommandType.StoredProcedure, ref parameters, commandTimeout);
        }

        protected DbCommand DbCommandConfig(
          string sqlText,
          CommandType commandType,
          ref UtilsParameter[] parameters,
          int? commandTimeout = null)
        {
            DbCommand command = RelationalDatabaseFacadeExtensions.GetDbConnection(this._ctx.Database).CreateCommand();

            if (command.Connection.State != ConnectionState.Open)
                RelationalDatabaseFacadeExtensions.OpenConnection(this._ctx.Database);
            if (this._transaction != null)
            {
                DbCommand dbCommand = command;
                IDbContextTransaction currentTransaction = this._ctx.Database.CurrentTransaction;
                DbTransaction dbTransaction = currentTransaction != null ? DbContextTransactionExtensions.GetDbTransaction(currentTransaction) : (DbTransaction)null;
                dbCommand.Transaction = dbTransaction;
            }
            command.Parameters.Clear();
            command.CommandText = sqlText;
            command.CommandType = commandType;
            command.CommandTimeout = commandTimeout ?? command.CommandTimeout;
            if (parameters != null && parameters.Length != 0)
            {
                foreach (UtilsParameter parameter1 in parameters)
                {
                    IDbDataParameter parameter2 = (IDbDataParameter)command.CreateParameter();
                    parameter2.ParameterName = parameter1.Name;

                    parameter2.Value = this.CustomValue(parameter1.Value);
                    parameter2.Direction = parameter1.Direction;
                    parameter2.Size = parameter1.Size;
                    command.Parameters.Add((object)parameter2);
                }
            }

            return command;
        }



        protected IEnumerable<T> DataReaderMultiple<T>(
          DbDataReader _DbDataReader)
        {
            List<T> objList = new List<T>();

            if (_DbDataReader.HasRows)
            {
                int fieldCount = _DbDataReader.FieldCount;
                Dictionary<int, string> dictionary = new Dictionary<int, string>();
                for (int index = 0; index < fieldCount; ++index)
                    dictionary.Add(index, _DbDataReader.GetName(index));
                object[] values = new object[fieldCount];
                while (_DbDataReader.Read())
                {
                    _DbDataReader.GetValues(values);
                    T instance = Activator.CreateInstance<T>();
                    foreach (PropertyInfo runtimeProperty in instance.GetType().GetRuntimeProperties())
                    {
                        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
                        {
                            if (!(runtimeProperty.Name.ToUpper() != keyValuePair.Value.ToUpper()))
                            {
                                object obj = values[keyValuePair.Key];
                                if (obj.GetType() != typeof(DBNull))
                                    runtimeProperty.SetValue((object)instance, obj, (object[])null);
                            }
                        }
                    }
                    objList.Add(instance);
                }
            }

            return (IEnumerable<T>)objList;
        }

        // END Reader múltiple
    }
}
