using RDBtoJSON.Extractors;
using System.Windows;

namespace RDBtoJSON.ParamControls
{
    /*
    server
    database
    user id
    password
    uint port
    */

    public partial class MySQLControl : IParamsExtractor
    {
        public IDataExtractor Extractor => new MySqlDataExtractor(txtDatabasePath.Text, txtDatabaseUser.Text, txtDatabasePass.Password, txtDatabaseServer.Text, uint.Parse(txtDatabasePort.Text));

        public MySQLControl() => InitializeComponent();

        private void txtDatabasePort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(this.txtDatabasePort.Text, out uint _))
                txtDatabasePort.Text = "3306";
        }
    }
}
