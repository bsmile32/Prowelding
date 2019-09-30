using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.DTO;

namespace DAL
{
    public class StockDal //: BaseDal
    {
        #region Init Dal

        private BaseDal conn;

        #region " | Instance | "

        private static volatile StockDal _instance;

        private StockDal()
        {
            conn = new BaseDal();
        }

        public static StockDal Instance
        {
            get
            {
                _instance = new StockDal();
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
        ~StockDal()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion | Dispose |

        #endregion

        public string InsertStock(List<InventoryDTO> lst, string User)
        {
            string err = "";
            try
            {
                List<SqlParameter> paramI = new List<SqlParameter>();
                foreach (InventoryDTO item in lst)
                {
                    paramI = new List<SqlParameter>();
                    paramI.Add(new SqlParameter() { ParameterName = "Remark", Value = item.Remark });
                    paramI.Add(new SqlParameter() { ParameterName = "ItemID", Value = item.ItemID, DbType = DbType.Int32 });
                    paramI.Add(new SqlParameter() { ParameterName = "Amount", Value = item.Amount, DbType = DbType.Int32 });
                    paramI.Add(new SqlParameter() { ParameterName = "Serial", Value = item.Serial });
                    paramI.Add(new SqlParameter() { ParameterName = "User", Value = User });
                    conn.ExcuteNonQueryNClose("InsertTransInboundNStock", paramI, out err);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return err;
        }

        public List<TransStock> CheckRemainingItem(Int32 ItemID)
        {
            List<TransStock> lst = new List<TransStock>();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "ItemID", Value = ItemID, DbType = DbType.Int32 });
                DataSet ds = conn.GetDataSet("GetItemRemaining", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    TransStock o = new TransStock();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        o = new TransStock();
                        o.StockID = Convert.ToInt32(dr["StockID"].ToString());
                        o.ItemID = Convert.ToInt32(dr["ItemID"].ToString());
                        o.Serial = dr["Serial"].ToString();
                        o.SaleHeaderID = Convert.ToInt32(dr["SaleHeaderID"].ToString());
                        o.SaleDetailID = Convert.ToInt32(dr["SaleDetailID"].ToString());
                        o.Active = dr["Active"].ToString();
                        lst.Add(o);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public List<InventoryDTO> GetItemStock(string ItemCode, string ItemName)
        {
            List<InventoryDTO> lst = new List<InventoryDTO>();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { ParameterName = "ItemName", Value = ItemName });
                param.Add(new SqlParameter() { ParameterName = "ItemCode", Value = ItemCode });
                DataSet ds = conn.GetDataSet("GetSearchStock", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    InventoryDTO o = new InventoryDTO();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        o = new InventoryDTO();
                        o.StockID = Convert.ToInt32(dr["StockID"].ToString());
                        o.ItemID = Convert.ToInt32(dr["ItemID"].ToString());
                        o.ItemCode = dr["ItemCode"].ToString();
                        o.ItemName = dr["ItemName"].ToString();
                        o.ItemDesc = dr["ItemDesc"].ToString();
                        o.Serial = dr["Serial"].ToString();
                        o.ItemPrice = Convert.ToDouble(dr["ItemPrice"].ToString());
                        o.UnitName = dr["UnitName"].ToString();
                        lst.Add(o);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public List<InventoryDTO> GetItemInStock()
        {
            List<InventoryDTO> lst = new List<InventoryDTO>();
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                //DataSet ds = conn.GetDataSet("GetSearchItemInStock", param);
                DataSet ds = conn.GetDataSet("GetSearchItem", param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    InventoryDTO o = new InventoryDTO();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        o = new InventoryDTO();
                        //o.StockID = Convert.ToInt32(dr["StockID"].ToString());
                        o.ItemID = Convert.ToInt32(dr["ItemID"].ToString());
                        o.ItemCode = dr["ItemCode"].ToString();
                        o.ItemName = dr["ItemName"].ToString();
                        o.ItemDesc = dr["ItemDesc"].ToString();
                        //o.Serial = dr["Serial"].ToString();
                        o.ItemPrice = Convert.ToDouble(dr["ItemPrice"].ToString());
                        o.UnitName = dr["UnitName"].ToString();
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
