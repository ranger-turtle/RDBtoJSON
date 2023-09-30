using System.Data.SqlClient;

namespace RDBtoJSON.Extractors
{
    internal class SqlServerDataExtractor : DbDataExtractor<SqlConnection, SqlCommand, SqlDataReader>
    {
        public SqlServerDataExtractor(string server, string path, string userId, string password)
        {
            var connString = new SqlConnectionStringBuilder
            {
                AttachDBFilename = CheckValue(path, "Nazwa pliku bazy danych nie może być pusta."),
                DataSource = CheckValue(server, "Nazwa serwera nie może być pusta."),
                UserID = userId ?? string.Empty,
                Password = password ?? string.Empty
            };

            this.Connection = new SqlConnection(connString.ConnectionString);
            this.Connection.Open();
            this.SqlCommand = new SqlCommand { Connection = this.Connection };
        }

        protected override int ColumnNumber => 0;
        protected override string Tables => "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
        protected override string GetColumns(string tableName) => $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
    }
}
