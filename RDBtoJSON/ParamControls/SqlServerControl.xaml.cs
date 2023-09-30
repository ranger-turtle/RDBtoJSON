using RDBtoJSON.Extractors;

namespace RDBtoJSON.ParamControls
{
    /*
    Server
    Database
    User Id
    Password
    */
    public partial class SqlServerControl : IParamsExtractor
    {
        public SqlServerControl() => InitializeComponent();

        public IDataExtractor Extractor => new SqlServerDataExtractor(txtDatabaseServer.Text, txtDatabasePath.Text, txtDatabaseUser.Text, txtDatabasePass.Password);
    }
}
