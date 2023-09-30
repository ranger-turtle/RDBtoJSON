using System.Data.SQLite;

namespace RDBtoJSON.Extractors
{
    internal class SQLiteDataExtractor : DbDataExtractor<SQLiteConnection, SQLiteCommand, SQLiteDataReader>
    {
        public SQLiteDataExtractor(string path, string password = "")
        {
            var connString = new SQLiteConnectionStringBuilder
            {
                DataSource = CheckValue(path, "Ścieżka do pliku bazy danych nie może być pusta."),
                Password = password ?? string.Empty
            };

            this.Connection = new SQLiteConnection(connString.ConnectionString);
            this.Connection.Open();
            this.SqlCommand = new SQLiteCommand { Connection = this.Connection };
        }

        protected override int ColumnNumber => 1;
        protected override string Tables => "SELECT name FROM sqlite_master WHERE type='table'";
        protected override string GetColumns(string tableName) => $"PRAGMA table_info('{tableName}')";
    }
}
