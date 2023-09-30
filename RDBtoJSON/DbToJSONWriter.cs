using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RDBtoJSON
{
    public static class DbToJSONWriter
	{
		private static StringBuilder json { get; } = new StringBuilder();
        private static int TabLevel { get; set; } = 1;
        private static string Tabs => new StringBuilder().Append('\t', TabLevel).ToString();

        private static void UpdateTabLevel(string value, int level)
        {
            if (level < 0)
                TabLevel += level;
            json.Append($"{Tabs}{value}\n");
            if (level > 0)
                TabLevel += level;
        }

        private static void SaveRows(List<Dictionary<string, object>> records)
		{
            string InQuotes(string value) => $"\"{value}\"";

            var lastRecord = records.Any() ? records.Last() : null;
			foreach (var record in records)
			{
                UpdateTabLevel("{", +1);
				foreach (var attrib in record)
				{
                    json.Append(Tabs + InQuotes(attrib.Key) + ": " + InQuotes(attrib.Value.ToString()));
                    json.Append(attrib.Equals(record.Last()) ? "\n" : ",\n");
				}
                var tabString = record.Equals(lastRecord) ? "}" : "},";
                UpdateTabLevel(tabString, -1);
			}
		}

		public static void SaveToJSON(List<Dictionary<string, object>> records, string name = "data.json", string query = "query")
		{
            using (var jsonFile = new StreamWriter(name, false, Encoding.UTF8))
            {
                json.Clear();
                json.Append("{\n");
                json.Append($"{Tabs}\"{query}\":\n");
                UpdateTabLevel("[", +1);
                SaveRows(records);
                UpdateTabLevel("]", -1);
                json.Append('}');
                jsonFile.Write(json.ToString());
            }
		}
	}
}
