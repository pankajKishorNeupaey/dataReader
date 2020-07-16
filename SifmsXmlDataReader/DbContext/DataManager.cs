using System;
using SifmsXmlDataReader.DbContext;
namespace SifmsXmlDataReader.DbContext
{
    public class MySqlConnection : AppConfiguration, System.IDisposable
    {

        //added by priyam
        public string DbConnection()
        {
            try
            {
                return _connectionString;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(ex, "", "", "", ErrorCodes.MySqlExceptionMsg(ex)));
                //throw new ApplicationException("Unable to get DB Connection string from Config File. Contact Administrator" + ex);
            }
        }

        //private static String _constr = DbConnection();


        // Pointer to an external unmanaged resource.
        private System.IntPtr handle;
        // Other managed resource this class uses.
        public MySql.Data.MySqlClient.MySqlConnection _MyConnection;
        // Track whether Dispose has been called.
        private bool disposed = false;
        public MySqlConnection()
        {
            try
            {
                _MyConnection = new MySql.Data.MySqlClient.MySqlConnection();
                _MyConnection.ConnectionString = DbConnection();
                _MyConnection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new System.Exception(ErrorCodes.ProcessException(SqEx, "", "", "", ErrorCodes.MySqlExceptionMsg(SqEx)));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_MyConnection != null)
                    {
                        if (_MyConnection.State == System.Data.ConnectionState.Open)
                            _MyConnection.Close();
                        _MyConnection.Dispose();
                    }
                }
                CloseHandle(handle);
                handle = System.IntPtr.Zero;
            }
            disposed = true;
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static System.Boolean CloseHandle(System.IntPtr handle);

        ~MySqlConnection()
        {
            Dispose(false);
        }
        public void Close()
        {
            Dispose();
        }
    }
    public class MySqlCommand : System.IDisposable
    {
        // Pointer to an external unmanaged resource.
        private System.IntPtr handle;
        // Other managed resource this class uses.
        private MySql.Data.MySqlClient.MySqlConnection _MyConnection;
        private MySql.Data.MySqlClient.MySqlDataAdapter _MyDataAdaptor;
        //private SqlDataReader _MyDatReader;
        private MySql.Data.MySqlClient.MySqlCommand _MyCommand;
        // Track whether Dispose has been called.
        private bool disposed = false;
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public MySqlCommand(MySql.Data.MySqlClient.MySqlConnection MyConnection)
        {
            _MyConnection = MyConnection;
            _MyCommand = new MySql.Data.MySqlClient.MySqlCommand();
            _MyCommand.Connection = _MyConnection;
        }
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            System.GC.SuppressFinalize(this);
        }
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    //if (_MyConnection != null)
                    //{
                    //    if (_MyConnection.State == ConnectionState.Open)
                    //        _MyConnection.Close();
                    //    _MyConnection.Dispose();
                    //    _MyConnection = null;
                    //}
                    if (_MyCommand != null)
                    {
                        if (_MyCommand.Parameters.Count > 0)
                            _MyCommand.Parameters.Clear();
                        _MyCommand.Dispose();
                    }
                    if (_MyDataAdaptor != null)
                    {
                        _MyDataAdaptor.Dispose();
                    }
                }
                // Release unmanaged resources. If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = System.IntPtr.Zero;
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.
            }
            disposed = true;
        }
        // Use interop to call the method necessary  
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static System.Boolean CloseHandle(System.IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~MySqlCommand()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        // Do not make this method virtual.
        // A derived class should not be allowed
        // to override this method.
        public void Close()
        {
            // Calls the Dispose method without parameters.
            Dispose();
        }
    

        /// <summary>
        /// Add a Parameter to current command object
        /// </summary>
        /// <param name="ParameterName">Parameter Name</param>
        /// <param name="Value">Parameter Value</param>
        public void Add_Parameter_WithValue(System.String ParameterName, System.Object Value)
        {
            try
            {
                if (ParameterName.IndexOf("@") < 0)
                    ParameterName = "@" + ParameterName;
                _MyCommand.Parameters.AddWithValue(ParameterName, Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new System.Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a Parameter to current command object
        /// </summary>
        /// <param name="ParameterName">Parameter Name</param>
        /// <param name="DataType">Parameter Data Type</param>
        /// <param name="Value">Parameter Value</param>
        public void Add_Parameter(System.String ParameterName, MySql.Data.MySqlClient.MySqlDbType DataType, System.Object Value)
        {
            try
            {
                if (ParameterName.IndexOf("@") < 0)
                    ParameterName = "@" + ParameterName;
                _MyCommand.Parameters.Add(ParameterName, DataType).Value = Value;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new System.Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a query or commandtext to current command object
        /// </summary>
        /// <param name="CommandText">Pass Command Text here </param>
        public void Add_CommandText(System.String CommandText)
        {
            try
            {
                if (CommandText != null)
                    _MyCommand.CommandText = CommandText;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new System.Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }


        /// <summary>
        /// Clears parameter from current command object
        /// </summary>
    
        public bool Clear_CommandParameter()
        {
            try
            {
                if (_MyCommand.Parameters.Count > 0)
                    _MyCommand.Parameters.Clear();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new System.Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a transaction locktype to the current commention
        /// </summary>
        /// <param name="MyTransaction">Pass Transaction object </param>
      
        public void Add_Transaction(MySql.Data.MySqlClient.MySqlTransaction MyTransaction)
        {
            try
            {
                _MyCommand.Transaction = MyTransaction;
            }
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(Sqex.Message);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Execute query in the databaxse
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or 
        /// StoredProcedure if you passed name of Stored Procedure as Query</param>
        private bool Execute_Query_WithTransaction(System.String Query, System.Data.CommandType CmdType, MySql.Data.MySqlClient.MySqlTransaction MyTransaction, System.Boolean UseTransaction)
        {
            try
            {
                if (_MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    _MyConnection.Open();
                    _MyCommand.Connection = _MyConnection;
                }
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CmdType;
                if (UseTransaction == true)
                    _MyCommand.Transaction = MyTransaction;
                _MyCommand.CommandTimeout = 0;
                _MyCommand.ExecuteNonQuery();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(Sqex, "", "", "", ErrorCodes.MySqlExceptionMsg(Sqex)));
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Execute query in the databaxse
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or 
        /// StoredProcedure if you passed name of Stored Procedure as Query</param>
        public bool Execute_Query(System.String Query, System.Data.CommandType CommandType)
        {
            try
            {
                if (_MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    _MyConnection.Open();
                    _MyCommand.Connection = _MyConnection;
                }
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CommandType;
                _MyCommand.CommandTimeout = 0;
                _MyCommand.ExecuteNonQuery();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(Sqex, "", "", "", ErrorCodes.MySqlExceptionMsg(Sqex)));
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }
        

        /// <summary>
        /// returns single string value from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public string Select_Scalar(string Query, System.Data.CommandType CommandType)
        {
            try
            {
                if (_MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    _MyConnection.Open();
                    _MyCommand.Connection = _MyConnection;
                }
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CommandType;
                _MyCommand.CommandTimeout = 0;
                return (System.Convert.ToString(_MyCommand.ExecuteScalar()));
            }
            
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(Sqex, "", "", "", ErrorCodes.MySqlExceptionMsg(Sqex)));
            }
            catch(System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        /// <summary>
        /// returns single datatable from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public System.Data.DataTable Select_Table(string Query, System.Data.CommandType CommandType)
        {
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                if (_MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    _MyConnection.Open();
                    _MyCommand.Connection = _MyConnection;
                }
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CommandType;
                _MyCommand.CommandTimeout = 0;

                _MyDataAdaptor = new MySql.Data.MySqlClient.MySqlDataAdapter(_MyCommand);
                _MyDataAdaptor.Fill(DT);
                return DT;
            }
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(Sqex, "", "", "", ErrorCodes.MySqlExceptionMsg(Sqex)));
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }
        /// <summary>
        /// returns one or more datatable from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public System.Data.DataSet Select_TableSet(string Query, System.Data.CommandType CommandType)
        {

            System.Data.DataSet DS = new System.Data.DataSet();
            try
            {
                if (_MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    _MyConnection.Open();
                    _MyCommand.Connection = _MyConnection;
                }
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CommandType;
                _MyCommand.CommandTimeout = 0;
                _MyCommand.CommandText = Query;
                _MyCommand.CommandType = CommandType;
                _MyCommand.CommandTimeout = 0;

                _MyDataAdaptor = new MySql.Data.MySqlClient.MySqlDataAdapter(_MyCommand);
                _MyDataAdaptor.Fill(DS);
                return DS;
            }
            
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new System.Exception(ErrorCodes.ProcessException(Sqex, "", "", "", ErrorCodes.MySqlExceptionMsg(Sqex)));
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(Ex.Message);
            }
        }

        /*
        * Added By: Alina Bhutia
        * Date: 20-June-2020
        * Description: this function is for binding output parameter that will be returned by called stored procedure
        */
        public MySql.Data.MySqlClient.MySqlParameter Add_Output_Parameter(string outParameterName)
        {
            MySql.Data.MySqlClient.MySqlParameter parameter = null;
            if (outParameterName.IndexOf("@") < 0)
            {
                outParameterName = "@" + outParameterName;
                parameter = new MySql.Data.MySqlClient.MySqlParameter(
                    outParameterName, MySql.Data.MySqlClient.MySqlDbType.Int64, 11)//change MysqlDbType if string is needed to be returned as output
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                _MyCommand.Parameters.Add(parameter);
                return parameter;
            }
            return parameter;
        }
    }
}
