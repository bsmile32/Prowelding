using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTO;

namespace DAL
{
    public class TransactionDal //: BaseDal
    {
        #region Init Dal

        private BaseDal conn;

        #region " | Instance | "

        private static volatile TransactionDal _instance;

        private TransactionDal()
        {
            conn = new BaseDal();
        }

        public static TransactionDal Instance
        {
            get
            {
                _instance = new TransactionDal();
                return _instance;
            }
        }

        #endregion

        #region | Dispose |

        private bool _disposed;
        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).

                // Set large fields to null.
                _disposed = true;
            }
        }

        // Destructor syntax for finalization code.
        ~TransactionDal()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion | Dispose |

        #endregion

        public List<SaleHeaderDTO> GetSearchCustomer(DateTime dtFrom, DateTime dtTo, string CusName)
        {
            List<SaleHeaderDTO> lst = new List<SaleHeaderDTO>();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "DateFrom", Value = dtFrom, DbType = DbType.DateTime });
                param.Add(new SqlParameter() { ParameterName = "DateTo", Value = dtTo, DbType = DbType.DateTime });
                param.Add(new SqlParameter() { ParameterName = "CustomerName", Value = CusName });
                DataSet ds = conn.GetDataSet("GetCustomerHistorty", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    SaleHeaderDTO o = new SaleHeaderDTO();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        o = new SaleHeaderDTO();
                        o.SaleHeaderID = Convert.ToInt32(dr["SaleHeaderID"].ToString());
                        o.CustomerName = dr["CustomerName"].ToString();   
                        o.Tel = dr["Tel"].ToString();
                        if (dr["ReceivedDate"].ToString() != "")
                        {
                            o.ReceivedDate = Convert.ToDateTime(dr["ReceivedDate"].ToString());
                        }
                        
                        o.ReceivedBy = dr["ReceivedBy"].ToString();
                        o.SaleNumber = dr["SaleNumber"].ToString();
                        o.ItemCode = dr["ItemCode"].ToString();
                        o.ItemName = dr["ItemName"].ToString();
                        o.ItemID = dr["ItemID"].ToString() == "" ? 0 : Convert.ToInt32(dr["ItemID"].ToString());
                        o.dAmount = Convert.ToDouble(dr["Amount"].ToString());
                        o.ItemPrice = Convert.ToDouble(dr["ItemPrice"].ToString());
                        o.Discount = Convert.ToDouble(dr["Discount"].ToString());
                        o.SerialNumber = dr["SerialNumber"].ToString();
                        o.BillType = dr["BillType"].ToString();

                        lst.Add(o);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }
    }
}
