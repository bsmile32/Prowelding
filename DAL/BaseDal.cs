using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDal : IDisposable
    {
        #region IDisposable Members

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseDal()
        {
            Dispose();
        }

        #endregion

        public SqlConnection _conn;
        private SqlTransaction _oraTrans;
        private SqlCommand _cmd;
        private SqlDataAdapter _adap;

        public BaseDal()
        {
            this._conn = new SqlConnection(GetConnectionString());
        }

        private string GetConnectionString()
        {
            string result = "", DB = "", Cat = "", User = "", Pass = "";
            DB = ConfigurationManager.AppSettings["dbsource"].ToString();
            Cat = ConfigurationManager.AppSettings["dbcatalog"].ToString();
            User = ConfigurationManager.AppSettings["dbuser"].ToString();
            Pass = ConfigurationManager.AppSettings["dbpass"].ToString();

            result = "Data Source=" + DB + ";" +
                     "Initial Catalog=" + Cat + ";" +
                     "User Id=" + User + ";" +
                     "Password=" + Pass + ";";
 
            return result;
        }

        public void GetConnection()
        {
            try
            {
                _conn = new SqlConnection(GetConnectionString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetDataSet(string StoredName, List<SqlParameter> param)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection conn = new SqlConnection(GetConnectionString());
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoredName;
                da.SelectCommand = cmd;

                foreach (SqlParameter item in param)
                {
                    cmd.Parameters.Add(item);
                }
                
                conn.Open();
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            finally 
            {
            }
            return ds;
        }

        public void ExcuteNonQueryNClose(string StoredName, List<SqlParameter> param, out string err)
        {
            try
            {
                err = "";
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoredName;

                foreach (SqlParameter item in param)
                {
                    cmd.Parameters.Add(item);
                }

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
            }
        }

        public void BeginTransaction()
        {
            try
            {
                _cmd = new SqlCommand();
                if (this._conn.State == ConnectionState.Closed) { this._conn.Open(); }
                this._oraTrans = this._conn.BeginTransaction(IsolationLevel.ReadCommitted);
                _cmd.Connection = this._conn;
                _cmd.Transaction = this._oraTrans;
            }
            catch (Exception ex)
            {
                throw new Exception("BeginTransaction: " + ex.Message, ex.InnerException);
            }
        }

        public void CallStoredProcedure(string storename, List<SqlParameter> parameters, out string err)
        {
            try
            {
                _cmd = this._conn.CreateCommand();
                if (this._conn.State == ConnectionState.Closed) { this._conn.Open(); }
                //_oraTrans = this._connection.BeginTransaction(IsolationLevel.ReadCommitted);
                //_cmd.Transaction = _oraTrans;

                _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                _cmd.CommandText = storename;
                //_cmd.BindByName = true;

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        _cmd.Parameters.Add(param);
                    }
                }
                //affected = _cmd.ExecuteNonQuery();
                _cmd.ExecuteNonQuery();
                //_oraTrans.Commit();
                //if (this._connection.State == ConnectionState.Open)
                //{
                //    this._connection.Close();
                //}
                err = null;
            }
            catch (SqlException ex)
            {
                RollBack();
                err = ex.Message;
            }
            catch (Exception ex)
            {
                RollBack();
                err = ex.Message;
            }
        }
        public void CallStoredProcedure(string storename, List<SqlParameter> parameters, out DataSet ds, out string err)
        {
            try
            {
                _cmd = this._conn.CreateCommand();
                if (this._conn.State == ConnectionState.Closed) { this._conn.Open(); }

                _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                _cmd.CommandText = storename;
                if (parameters != null)
                {
                    //foreach (string key in parameters)
                    foreach (SqlParameter param in parameters)
                    {
                        if (param.Direction == ParameterDirection.Output && param.DbType == DbType.String)
                        {
                            param.Size = 4000;
                        }
                        _cmd.Parameters.Add(param);
                    }
                }

                _adap = new SqlDataAdapter(_cmd);
                ds = new DataSet();
                _adap.Fill(ds);

                err = null;
            }
            catch (SqlException ex)
            {
                ds = null;
                RollBack();
                err = ex.Message;
            }
            catch (Exception ex)
            {
                ds = null;
                RollBack();
                err = ex.Message;
            }
        }
        public void Commit()
        {
            try
            {
                this._oraTrans.Commit();
                if (this._conn.State == ConnectionState.Open) { this._conn.Close(); }
                Dispose();
            }
            catch (Exception ex)
            {
                Dispose();
                if (this._conn.State == ConnectionState.Open) { this._conn.Close(); }
                throw new Exception("Commit: " + ex.Message);
            }
        }
        public void RollBack()
        {
            try
            {
                this._oraTrans.Rollback();
                if (this._conn.State == ConnectionState.Open) { this._conn.Close(); }
                Dispose();
            }
            catch (Exception ex)
            {
                Dispose();
                if (this._conn.State == ConnectionState.Open) { this._conn.Close(); }
                throw new Exception("RollBack: " + ex.Message);
            }
        }
    }
}
