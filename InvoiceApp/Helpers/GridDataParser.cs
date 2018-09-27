using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace InvoiceApp.Helpers
{
    public class GridDataParser
    {
        #region "Private variables"

        private readonly ILogger _logger;

        #endregion "Private variables"
        /// <summary>
        /// This method used to convert list item to data tables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            if (dataTable.Columns.Contains("ExtensionData"))
            {
                dataTable.Columns.Remove("ExtensionData");
            }
             
            return dataTable;
        }
        /// <summary>
        /// To convert Datatable to dictionary
        /// </summary>
        /// <param name="resulDataTable"></param>
        /// <returns></returns>
        public static List<Dictionary<object, object>> DataTableToDictionary(DataTable resulDataTable)
        {
            return (from DataRow dataRow in resulDataTable.Rows select resulDataTable.Columns.Cast<DataColumn>().ToDictionary<DataColumn, object, object>(column => column.ColumnName, column => (dataRow[column].ToString() != "") ? dataRow[column].ToString() : "")).ToList();
        }
        /// <summary>
        /// To convert list columns name as Datatable  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="displayColumns"></param>
        /// <returns></returns>
        public List<Dictionary<object, object>> TranformGridData<T>(List<T> items, string displayColumns)
        {
            var lstItems = new List<Dictionary<object, object>>();
            try
            {
                DataTable dtResultTable = ListToDataTable(items);

                //Column Ordering
                string[] columnOrder = displayColumns.Split('@')[0].Split(',');
                string[] columnNames = displayColumns.Split('@')[1].Split(',');

                int columnIndex = 0;
                foreach (var column in columnOrder)
                {
                    dtResultTable.Columns[column].SetOrdinal(columnIndex);
                    dtResultTable.Columns[column].ColumnName = columnNames[columnIndex];                  

                    //if (columnOrder.Count() > columnIndex + 1)
                    columnIndex++;
                }

     

                lstItems = DataTableToDictionary(dtResultTable);

                return lstItems;

            }
            catch (Exception ex)
            {
                _logger.Error("GetApplicationList, error=" + ex.Message);
                return lstItems;
            }

        }

    }
}