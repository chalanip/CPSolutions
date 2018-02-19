using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataUtility.SQL
{
    /// <summary>
    /// Contains the routines to work with SQL (RDBMS) databases.
    /// </summary>
    public interface ISql
    {
        /// <summary>
        /// Create a db connection.
        /// </summary>
        /// <returns>Created db connection.</returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Create a db connection.
        /// </summary>
        /// <param name="connectionString">Connection string to Database.</param>
        /// <returns>Created db connection.</returns>
        IDbConnection GetConnection(string connectionString);

        /// <summary>
        /// Test db connection.
        /// </summary>
        /// <returns>True if connected to db, otherwise False.</returns>
        bool TestConnection();

        /// <summary>
        /// Create DataAdapter.
        /// </summary>
        /// <returns>Created DataAdapter.</returns>
        IDbDataAdapter GetDataAdapter();

        /// <summary>
        /// Execute a query and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Number of rows affected.</returns>
        int ExecuteQuery(string commandText, params KeyValuePair<string, dynamic>[] parameters);

        /// <summary>
        /// Execute a stored procedure and returns the number of rows affected.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Number of rows affected.</returns>
        int ExecuteStoredProcedure(string storedProcedureName, params KeyValuePair<string, dynamic>[] parameters);

        /// <summary>
        /// Execute a query that return a scalar value (single value).
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>First column of the first row in the resultset (single value).</returns>
        object ExecuteScaler(string commandText, params KeyValuePair<string, dynamic>[] parameters);

        /// <summary>
        /// Execute a query that return a DataSet.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Dataset result.</returns>
        DataSet ExecuteDataSet(string commandText, params KeyValuePair<string, dynamic>[] parameters);

        /// <summary>
        /// Execute a query that return a DataTable.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="startIndex">The zero based record number to start with.</param>
        /// <param name="pageSize">The maximum number of records to retrieve.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>Datatable result</returns>
        DataTable ExecuteDataTable(string commandText, int? startIndex = null, int? pageSize = null, params KeyValuePair<string, dynamic>[] parameters);

        /// <summary>
        /// Execute a query that return a DataReader.
        /// </summary>
        /// <param name="commandText">Command text to be executed.</param>
        /// <param name="parameters">Parameters if available.</param>
        /// <returns>DataReader of the result.</returns>
        IDataReader ExecuteReader(string commandText, params KeyValuePair<string, dynamic>[] parameters);

       
    }
}
