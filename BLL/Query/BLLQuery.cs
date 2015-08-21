using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using System.Data;
using System.Text.RegularExpressions;


namespace BLL
{
    public class BLLQuery
    {
        IDataAccess da;

        public BLLQuery()
        {
            da = IDAL.DALFactory.GetDataAccess();
        }
        public BLLQuery(string CnKey)
        {
            da = DALFactory.GetDataAccess(CnKey);
        }

        public DataTable GetWareHouseQuery(string ShelfCode,string AreaCode)
        {
            DataTable dt;
            if (ShelfCode != "")
            {
                dt = da.FillDataTable("CMD.SelectWareHouseCellQueryByShelf", new DataParameter[] { new DataParameter("@ShelfCode", ShelfCode) });
            }
            else
            {
                dt = da.FillDataTable("CMD.SelectWareHouseCellQueryByArea", new DataParameter[] { new DataParameter("@AreaCode", AreaCode) });
            }
            return dt;
        }
        public DataTable GetWareHouseQuery(string ShelfCode, string AreaCode,string ShelfWhere)
        {
            DataTable dt;
            if (ShelfCode != "")
            {
                dt = da.FillDataTable("CMD.SelectWareHouseCellQueryByShelf", new DataParameter[] { new DataParameter("@ShelfCode", ShelfCode) });
            }
            else
            {
                if (ShelfWhere == "")
                {
                    ShelfWhere = "1=1";
                }
                dt = da.FillDataTable("CMD.SelectWareHouseCellQueryByAreaShelfWhere", new DataParameter[] { new DataParameter("@AreaCode", AreaCode), new DataParameter("{0}", ShelfWhere) });
            }
            return dt;
        }
        public DataTable GetProductQuery(string StartTime, string EndTime, string ProductWhere, string UserName)
        {
            DataParameter[] paras = new DataParameter[] {
                                                            new DataParameter("@StartTime", StartTime),
                                                            new DataParameter("@EndTime", EndTime),
                                                            new DataParameter("@UserName", UserName),
                                                            new DataParameter("@ProductWhere",ProductWhere)
                                                         };
            DataTable dt = da.FillDataTable("CMD.SPProductQuery", paras);
            return dt;
        }
        public DataTable GetProductQuery(string StartTime, string EndTime, string ProductWhere, string UserName,int StockID)
        {
            DataParameter[] paras = new DataParameter[] {
                                                            new DataParameter("@StartTime", StartTime),
                                                            new DataParameter("@EndTime", EndTime),
                                                            new DataParameter("@UserName", UserName),
                                                            new DataParameter("@ProductWhere",ProductWhere),
                                                            new DataParameter("@StockID",StockID)
                                                         };
            DataTable dt = da.FillDataTable("CMD.SPOtherProductQuery", paras);
            return dt;
        }

        public DataTable GetOutStockQuery(string strWhere, string FactWhere, string CustWhere,bool blnTotal)
        {
            DataParameter[] paras = new DataParameter[] {
                                                            new DataParameter("{0}", strWhere),
                                                            new DataParameter("{1}", FactWhere),
                                                            new DataParameter("{2}", CustWhere ) 
                                                         };
            string Comds = "WMS.SelectOutStockQuery";
            if(blnTotal)
                Comds = "WMS.SelectOutStockTotalQuery";
            DataTable dt = da.FillDataTable(Comds, paras);
            return dt;
        }

        public DataTable GetInStockQuery(string strWhere, string FactWhere, string CustWhere, bool blnTotal)
        {
            DataParameter[] paras = new DataParameter[] {
                                                            new DataParameter("{0}", strWhere),
                                                            new DataParameter("{1}", FactWhere),
                                                            new DataParameter("{2}", CustWhere ) 
                                                         };
            string Comds = "WMS.SelectInStockQuery";
            if (blnTotal)
                Comds = "WMS.SelectInStockTotalQuery";
            DataTable dt = da.FillDataTable(Comds, paras);
            return dt;
        }
    }
}
