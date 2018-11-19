using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace Courby.Data
{
    /// <summary>
    /// Provides Sql Connections.
    /// </summary>
    public static class Connection
    {
        #region Members 

        static List<SqlConnection> _conns = new List<SqlConnection>();
        static int _closeCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current connection count.
        /// </summary>
        public static int ConnectionCount
        {
            get
            {
                return _conns.Count; 
            }
        }

        #endregion

        #region Structures

        /// <summary>
        /// Parameter Data Structure
        /// </summary>
        public struct ParamData
        {
            public string name;
            public object value;
            public bool output;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Executes a procedure. 
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static DataStream ExecuteProcedureDataSet(string procName, params ParamData[] variables)
        {
            return new DataStream(ExecuteProcedure(procName, variables));
        }

        /// <summary>
        /// Executes a stored procedure against the Active Connection
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteProcedure(string procName, params ParamData[] variables)
        {
            SqlCommand sqlCmd = PrepareCommand(procName, BuildParameters(variables));

            return sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Executes a non query based (no result set) stored procedure
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static int ExecuteProcedureNonQuery(string procName, params ParamData[] variables)
        {
            //SqlCommand sqlCmd = PrepareCommand(procName, BuildParameters(variables));
            //return sqlCmd.ExecuteNonQuery();
            SqlParameter[] result =  ExecuteProcedureNonQuery(procName, false, variables);

            for (int i = result.Length; i >= 0; i--)
                if (result[i].ParameterName == "@RETURN") return (int)result[i].Value;

            return -1;
        }

        /// <summary>
        /// Executes a non resultset return. With returning output parameters.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static SqlParameter[] ExecuteProcedureNonQuery(string procName, bool useOutputParams, params ParamData[] variables)
        {
            List<ParamData> outputParams = new List<ParamData>();

            SqlCommand sqlCmd = PrepareCommand(procName, BuildParameters(variables));
            sqlCmd.Parameters.Add(PrepareReturnParameter());

            sqlCmd.ExecuteNonQuery();

            SqlParameter[] returnedItems = new SqlParameter[sqlCmd.Parameters.Count];
            sqlCmd.Parameters.CopyTo(returnedItems, 0);

            return returnedItems;
        }

        /// <summary>
        /// Builds the params for SQL
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SqlParameter[] BuildParameters(params ParamData[] data)
        {
            List<SqlParameter> paramsList = new List<SqlParameter>();

            for (int i = 0; i < data.Length; i++)
            {
                ParamData item = data[i];

                paramsList.Add(PrepareParameter(item.name, item.value, item.output));
            }

            return paramsList.ToArray();
        }

        /// <summary>
        /// Builds the command object.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        static SqlCommand PrepareCommand(string procName, params SqlParameter[] variables)
        {
            SqlConnection conn = GetConnection();

            SqlCommand sqlCmd = new SqlCommand(procName, conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            if (variables != null)
            {
                sqlCmd.Parameters.AddRange(variables);
            }

            if (conn.State == System.Data.ConnectionState.Closed || conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Open();
            }

            return sqlCmd;
        }

        /// <summary>
        /// Gets the SqlParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static SqlParameter PrepareParameter(string name, object value)
        {
            return PrepareParameter(name, value, false);
        }
        /// <summary>
        /// Gets the SqlParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="output">True if output parameter</param>
        /// <returns></returns>
        static SqlParameter PrepareParameter(string name, object value, bool output)
        {
            SqlParameter param = new SqlParameter(name, value);

            if (output) param.Direction = System.Data.ParameterDirection.InputOutput;

            if (value == null)
                param.Value = DBNull.Value;

            return param;
        }

        /// <summary>
        /// Creates return param.
        /// </summary>
        /// <returns>SqlParameter - Set for return parameter</returns>
        static SqlParameter PrepareReturnParameter() 
        {
            SqlParameter retParam = PrepareParameter("@RETURN", 0, true);

            retParam.Direction = System.Data.ParameterDirection.ReturnValue;

            return retParam;
        }

        /// <summary>
        /// Gets a Connection to the database and stores in the connection collection for later disposal.
        /// </summary>
        /// <returns></returns>
        static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

            conn.StateChange += ConnStateChange;

            lock(_conns)
            {
                _conns.Add(conn);
            }

            return conn;
        }

        /// <summary>
        /// Remove SqlConnection Instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ConnStateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            SqlConnection obj = (SqlConnection)sender;

            if (e.CurrentState == System.Data.ConnectionState.Closed || e.CurrentState == System.Data.ConnectionState.Broken)
            {
                lock (_conns)
                { 
                    _conns.Remove(obj);
                    Console.WriteLine("** Connection Closed ** " + ConnectionCount + " " + ++_closeCount);
                }
            }
        }


        #endregion
    }
}
