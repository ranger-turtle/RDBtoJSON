using RDBtoJSON.Extractors;
using System;
using System.Windows;

namespace RDBtoJSON.ParamControls
{
    /*
    database path (db, db2, db3, sqlite, sqlite2, sqlite3, sdb, s2db, s3db)
    password
    */
    public partial class SQLiteControl : IParamsExtractor
    {
        public SQLiteControl() => InitializeComponent();
        public IDataExtractor Extractor => new SQLiteDataExtractor(txtDatabasePath.Text, txtDatabasePass.Password);

        private void DatabaseSelect_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "db files (db, db2, db3, sqlite, sqlite2, sqlite3, sdb, s2db, s3db)|*.db;*.db2;*.db3;*.sqlite;*.sqlite2;*.sqlite3;*.sdb;*.s2db;*.s3db",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true
            };

            if (ofd.ShowDialog() == true)
                this.txtDatabasePath.Text = ofd.FileName;   //strange ext bug when added by hand
        }  
    }
}
