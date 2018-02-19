using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace CP.DataUtility.SQL
{
    /// <summary>
    /// API class to expose all SQL (RDBMS) routines. 
    /// </summary>
    public static class Sql
    {
        //private static readonly ISql _sql;

        public const string MYSQL_DB_PROVIDER = "MySql.Data.MySqlClient";
        //static string _connectionString = "Server=entrusttitle.ddns.net;Database=chalanitest;Uid=zsdevlocaluser;Pwd=zorroSign123;Allow User Variables=True;Pooling=True;CHARSET=utf8;";
        static string _connectionString = ConfigurationManager.AppSettings["connectionString"];

        static Sql(){}
        
        /// <summary>
        /// Create a db connection with given connection data.
        /// </summary>
        /// <returns>Created db connection.</returns>
        public static IDbConnection GetConnection()
        {
            var connection = DbProviderFactories.GetFactory(MYSQL_DB_PROVIDER).CreateConnection();
            if (connection == null) return null;

            connection.ConnectionString = _connectionString;
            return connection;
        }

        /// <summary>
        /// Create a db connection.
        /// </summary>
        /// <param name="connectionStringName">Connection String</param>
        /// <returns>Created db connection.</returns>
        public static IDbConnection GetConnection(string connectionStringName)
        {
            var connection = DbProviderFactories.GetFactory(MYSQL_DB_PROVIDER).CreateConnection();

            if (connection == null) return null;

            connection.ConnectionString = connectionStringName;

            return connection;
        }


        /// <summary>
        /// Test db connection.
        /// </summary>
        /// <returns>True if connected to db, otherwise False.</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Error in DB connection test", ex);
                return false; // any error is considered as db connection error for now
            }
        }

        /// <summary>
        /// Create DataAdapter.
        /// </summary>
        /// <returns>Created DataAdapter.</returns>
        public static IDbDataAdapter GetDataAdapter()
        {
            string providerName = MYSQL_DB_PROVIDER;
            DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);
            var dataAdaptor = provider.CreateDataAdapter();

            return dataAdaptor;

        }

        /// <summary>
        /// Execute a query and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Number of rows affected.</returns>
        public static int ExecuteQuery(string commandText, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;

                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                command.CommandType = CommandType.Text;
                command.Connection.Open();
                var result = command.ExecuteNonQuery();
                command.Connection.Close();

                return result;
            }


        }

        /// <summary>
        /// Execute a stored procedure and returns the number of rows affected.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Number of rows affected.</returns>
        public static int ExecuteStoredProcedure(string storedProcedureName, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
                throw new ArgumentException("Stored procedure name is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = storedProcedureName;

                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                command.CommandType = CommandType.StoredProcedure;
                command.Connection.Open();
                var result = command.ExecuteNonQuery();
                command.Connection.Close();

                return result;
            }


        }

        /// <summary>
        /// Execute a query that return a scalar value (single value).
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>First column of the first row in the resultset (single value).</returns>
        public static object ExecuteScaler(string commandText, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                command.Connection.Open();
                var result = command.ExecuteScalar();
                command.Connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Execute a query that return a DataSet.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Dataset result.</returns>
        public static DataSet ExecuteDataSet(string commandText, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                var result = new DataSet();

                command.CommandText = commandText;
                command.Connection.Open();
                var dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(result);
                command.Connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Execute a query that return a DataTable.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="startIndex">The zero based record number to start with.</param>
        /// <param name="pageSize">The maximum number of records to retrieve.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Datatable result</returns>
        public static DataTable ExecuteDataTable(string commandText, int? startIndex = null, int? pageSize = null, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                var result = new DataTable();

                command.CommandText = commandText;
                command.Connection.Open();
                DbDataAdapter dataAdapter = (DbDataAdapter)GetDataAdapter();
                dataAdapter.SelectCommand = (DbCommand)command;
                if (startIndex == null && pageSize == null)
                {
                    dataAdapter.Fill(result);
                }
                else
                {
                    dataAdapter.Fill(startIndex.GetValueOrDefault(), pageSize.GetValueOrDefault(), result);
                }
                command.Connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Execute a query that return a DataReader.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>DataReader of the result.</returns>
        public static IDataReader ExecuteReader(string commandText, params KeyValuePair<string, dynamic>[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText is empty.");

            var connection = GetConnection();

            using (var command = connection.CreateCommand())
            {
                foreach (var parameter in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                command.CommandText = commandText;
                command.Connection.Open();

                return command.ExecuteReader();
            }
        }



        #region private methods

        /// <summary>
        /// Generate connection string with the given properties
        /// </summary>
        /// <param name="connectionDto">Connection details</param>
        /// <returns>Connection string of the database</returns>
        private static string GenerateConnectionString(ConnectionDto connectionDto)
        {
            DbConnectionStringBuilder connectionBuilder = null;

            string connectionString = string.Empty;
            connectionBuilder = new MySqlConnectionStringBuilder();

            if (connectionBuilder != null)
            {
                // Populate connection builder with connection properties
                foreach (KeyValuePair<string, string> property in connectionDto.Properties)
                {
                    if (connectionBuilder.ContainsKey(property.Key) && !string.IsNullOrEmpty(property.Value))
                    {
                        connectionBuilder.Add(property.Key, property.Value);
                    }
                }
                connectionString = connectionBuilder.ConnectionString;
                connectionDto.ConnectionString = connectionString;
            }
            return connectionString;
        }


        #endregion private methods
    }
}
