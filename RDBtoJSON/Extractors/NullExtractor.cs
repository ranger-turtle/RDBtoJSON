using System;
using System.Collections.Generic;

namespace RDBtoJSON.Extractors
{
    internal class NullExtractor : IDataExtractor
    {
        Exception DbNotSelected = new Exception("Baza danych nie została jeszcze wybrana.");

        public void CloseResources() { }

        public List<Dictionary<string, object>> GetRows(string query) => throw DbNotSelected;

        public Dictionary<string, List<string>> GetTableData() => throw DbNotSelected;
    }
}
