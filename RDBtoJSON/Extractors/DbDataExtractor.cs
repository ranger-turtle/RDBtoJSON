using System;
using System.Collections.Generic;
using System.Data.Common;

namespace RDBtoJSON.Extractors
{
    internal abstract class DbDataExtractor<Cn, Cm, R> : IDataExtractor
    where Cn : DbConnection
    where Cm : DbCommand
    where R : DbDataReader
    {
        protected Cn Connection { get; set; }
        protected Cm SqlCommand { get; set; }

        protected abstract string Tables { get; }
        protected abstract int ColumnNumber { get; }
        protected abstract string GetColumns(string tableName);

        protected string CheckValue(string value, string message) { return string.IsNullOrEmpty(value) ? throw new ArgumentException(message) : value; }

        public Dictionary<string, List<string>> GetTableData()
        {
            var tableData = new Dictionary<string, List<string>>();
            var tableNames = new List<string>();
            SqlCommand.CommandText = Tables;
            using (var dataReader = SqlCommand.ExecuteReader() as R)
            {
                while (dataReader.Read())
                    tableNames.Add(dataReader.GetValue(0).ToString());
            }
            foreach (string tableName in tableNames)
            {
                var columnList = new List<string>();
                SqlCommand.CommandText = GetColumns(tableName);
                using (var dataReader = SqlCommand.ExecuteReader() as R)
                {
                    while (dataReader.Read())
                        columnList.Add(dataReader[ColumnNumber].ToString());
                }
                tableData.Add(tableName, columnList);
            }
            return tableData;
        }

        public List<Dictionary<string, object>> GetRows(string query)
        {
            var records = new List<Dictionary<string, object>>();
            if (!query.ToUpper().Trim().StartsWith("SELECT"))
                throw new Exception("Można wybrać tylko zapytanie pobierające dane (SELECT).");
            SqlCommand.CommandText = query;
            using (var dataReader = SqlCommand.ExecuteReader() as R)
            {
                while (dataReader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                        row.Add(dataReader.GetName(i), dataReader.GetValue(i).ToString());
                    records.Add(row);
                }
            }
            return records;
        }

        public void CloseResources()
        {
            SqlCommand.Dispose();
            Connection.Close();
        }

    }
}
