using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HRMS.UI.Areas.Transaction.Models
{
    public class ItemStockCollection : List<ItemOpeningStockType>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("ItemId", SqlDbType.Int),
                  new SqlMetaData("Quantity", SqlDbType.Decimal),
                  new SqlMetaData("TranId", SqlDbType.VarChar, 50),
                  new SqlMetaData("TranDate", SqlDbType.Date));

            foreach (ItemOpeningStockType cust in this)
            {
                sqlRow.SetSqlInt32(0, cust.ItemId);
                sqlRow.SetDecimal(1, cust.Quantity);
                sqlRow.SetString(2, cust.TranId);
                sqlRow.SetDateTime(3, cust.TranDate);
                yield return sqlRow;
            }
        }
    }
}