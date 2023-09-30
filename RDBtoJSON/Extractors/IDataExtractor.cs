using System.Collections.Generic;

namespace RDBtoJSON.Extractors
{
    public interface IDataExtractor
    {
        Dictionary<string, List<string>> GetTableData();
        List<Dictionary<string, object>> GetRows(string query);
        void CloseResources();
    }
}
