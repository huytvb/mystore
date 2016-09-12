using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using MySql.Data.MySqlClient;
using MyStore.DataAccess.Interface;
using MyStore.DataAccess.Mapping;

namespace MyStore.DataAccess
{
    public class EfContext : DbContext, IDbContext
    {
        private readonly Lazy<EntityQueryFilterProvider> _filterProviderInitializer = new Lazy<EntityQueryFilterProvider>();
        public EfContext()
            : base("MyStoreConnection")
        {

        }

        public EfContext(DbConnection connection)
            : base(connection, false)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
#pragma warning disable 612,618
            modelBuilder.Conventions
                .Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();

#pragma warning disable 612,618
            modelBuilder.Conventions.Remove
                <System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            string databaseType;
            var dbConnection = Database.Connection;
            databaseType = dbConnection is SqlConnection ?
                "SqlServer" : dbConnection is MySqlConnection
                ? "MySql" : "Oracle";
            var configType = typeof(UserMapMySql);
            var typeToRegister = Assembly.GetAssembly(configType).GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace)
                    && (type.Namespace.Equals("MyStore.DataAccess.Mapping"))
                    && type.FullName.EndsWith("MySql", StringComparison.InvariantCultureIgnoreCase))
                    .Where(type => type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typeToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EfRepository<T>(this);
        }

        /// <summary>
        /// Sử dụng câu query sql để thay đổi dữ liệu
        /// </summary>
        /// <param name="commandText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns></returns>
        public int RawModify(string commandText, params object[] parameters)
        {
            int result;
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    result = command.ExecuteNonQuery();
                }
            }
            finally
            {
                Database.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Sử dụng câu query sql để truy vấn dữ liệu
        /// </summary>
        /// <param name="queryText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Danh sách kết quả truy vấn</returns>
        public IEnumerable<dynamic> RawQuery(string queryText, params object[] parameters)
        {
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = queryText;
                    command.Connection = Database.Connection;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.ToExpandoList();
                    }
                }
            }
            finally
            {
                Database.Connection.Close();
            }
        }

        /// <summary>
        /// Sử dụng câu store để truy vấn dữ liệu
        /// </summary>
        /// <param name="storeName">Câu store.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Danh sách kết quả truy vấn</returns>
        public IEnumerable<dynamic> RawProcedure(string storeName, params object[] parameters)
        {
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = storeName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = Database.Connection;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.ToExpandoList();
                    }
                }
            }
            finally
            {
                Database.Connection.Close();
            }
        }

        /// <summary>
        /// Sử dụng câu query sql để truy vấn dữ liệu (lấy ra dữ liệu của cột đầu tiên dòng đầu tiên)
        /// </summary>
        /// <param name="commandText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns></returns>
        public object RawScalar(string commandText, params object[] parameters)
        {
            object result;
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    result = command.ExecuteScalar();
                }
            }
            finally
            {
                Database.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Sử dụng câu sql để truy vấn dữ liệu
        /// </summary>
        /// <param name="commandText">Sql query</param>
        /// <param name="parameters">Sql parameter</param>
        /// <returns>Trả về kết quả dạng datatable</returns>
        public DataTable RawTable(string commandText, params object[] parameters)
        {
            DataTable result;
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    result = command.ExecuteReader().ToDataTable();
                }
            }
            finally
            {
                Database.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Sử dụng Store để truy vấn dữ liệu
        /// </summary>
        /// <param name="commandText">Sql query</param>
        /// <param name="parameters">Sql parameter</param>
        /// <returns>Trả về kết quả dạng datatable</returns>
        public DataTable RawProcedureTable(string commandText, params object[] parameters)
        {
            DataTable result;
            try
            {
                using (var command = Database.Connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter is DbParameter)
                            {
                                var param = parameter as DbParameter;
                                var newParam = command.CreateParameter();
                                newParam.DbType = param.DbType;
                                newParam.Direction = param.Direction;
                                newParam.IsNullable = param.IsNullable;
                                newParam.ParameterName = param.ParameterName;
                                newParam.Size = param.Size;
                                newParam.SourceColumn = param.SourceColumn;
                                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                                newParam.SourceVersion = param.SourceVersion;
                                newParam.Value = param.Value;
                                command.Parameters.Add(newParam);
                            }
                            else
                            {
                                command.AddParams(parameter);
                            }
                        }
                    }
                    Database.Connection.Open();
                    result = command.ExecuteReader().ToDataTable();
                }
            }
            finally
            {
                Database.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Các bộ lọc cho từng nguồn dữ liệu
        /// </summary>
        /// <value>Các bộ lọc.</value>
        public new System.Data.Entity.Infrastructure.DbContextConfiguration Configuration
        {
            get { return base.Configuration; }
        }

        public QueryFilterProvider Filters
        {
            get { return _filterProviderInitializer.Value; }
        }
    }
}