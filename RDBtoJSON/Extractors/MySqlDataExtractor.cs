using MySql.Data.MySqlClient;

namespace RDBtoJSON.Extractors
{
    internal class MySqlDataExtractor : DbDataExtractor<MySqlConnection, MySqlCommand, MySqlDataReader>
    {
        public MySqlDataExtractor(string databaseName, string userId, string password, string server = "localhost", uint port = 3306)
        {
            var connString = new MySqlConnectionStringBuilder
            {
                Server = CheckValue(server, "Nazwa serwera nie może być pusta."),
                Port = port,
                Database = CheckValue(databaseName, "Nazwa bazy danych nie może być pusta."),
                UserID = CheckValue(userId, "Login nie może być pusty."),
                Password = password ?? string.Empty
            };

            this.Connection = new MySqlConnection(connString.ConnectionString);
            this.Connection.Open();
            this.SqlCommand = new MySqlCommand { Connection = this.Connection };
        }

        protected override int ColumnNumber => 0;
        protected override string Tables => "SHOW TABLES";
        protected override string GetColumns(string tableName) => $"SHOW COLUMNS FROM {tableName}";
    }
}
