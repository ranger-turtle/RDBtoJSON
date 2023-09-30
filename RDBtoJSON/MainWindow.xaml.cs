using RDBtoJSON.Extractors;
using RDBtoJSON.ParamControls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;

namespace RDBtoJSON
{
    public partial class MainWindow
    {
		private IParamsExtractor ChosenDatabase { get; set; }
		private IDataExtractor DataExtractor { get; set; } = new NullExtractor();
        private List<Dictionary<string, object>> QueryResult { get; set; } = new List<Dictionary<string, object>>();
        private Dictionary<string, List<string>> DbTables { get; set; } = new Dictionary<string, List<string>>();
        private bool Connected { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            cmbDbType.SelectionChanged += cmbDbTypeSelectionChanged;
            cmbDbType.SelectedIndex = 0;
			ChosenDatabase = ParamsSqlServer;
        }

        private void Window_Closed(object sender, EventArgs e) => DataExtractor.CloseResources();

        private void cmbDbTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var elems = new UserControl[] { ParamsSqlServer, ParamsSQLite, ParamsMySQL };
            elems.OnEach(c => c.Visibility = Visibility.Collapsed);
            elems[cmbDbType.SelectedIndex].Visibility = Visibility.Visible;
			ChosenDatabase = elems[cmbDbType.SelectedIndex] as IParamsExtractor;
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                this.Connected = false;
                await Task.Run(async () => {
                    DataExtractor.CloseResources();
                    await Dispatcher.InvokeAsync(() => DataExtractor = ChosenDatabase.Extractor);
                    this.DbTables = DataExtractor.GetTableData();
                });
                viewTables.Items.Clear();
                this.DbTables.OnEach(kv => {
                    var item = new TreeViewItem { Header = kv.Key };
                    kv.Value.OnEach(column => item.Items.Add(column));
                    viewTables.Items.Add(item);
                });
                this.Connected = true;
            }
            catch (Exception ex)
            {
                var mess = (ex is DbException) ? $"Pojawił się błąd bazy danych:\n{ex.Message}" : ex.Message;
                MessageBox.Show(mess, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
		    if (!this.Connected)
		    {
		        MessageBox.Show("Brak połączenia z bazą danych.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
		        return;
		    }

		    var ofd = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "JSON file|*.json",
                FileName = "data",
                AddExtension = true,
                ValidateNames = true
            };
		    if (ofd.ShowDialog() != true)
		        return;

		    try
		    {
		        DbToJSONWriter.SaveToJSON(this.QueryResult, ofd.FileName, txtQuery.Text);
		        MessageBox.Show("Pomyślnie zapisano wyniki.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
		    }
		    catch (Exception ex)
		    { 
		        MessageBox.Show($"Pojawił się błąd przy zapisie danych:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
		    }
		}

        private void btnPerformQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtQuery.Text))
                    throw new Exception("Najpierw musisz wpisać zapytanie.");

                this.QueryResult = DataExtractor.GetRows(txtQuery.Text);
                viewQuery.Items.Clear();
                int counter = 0;
                this.QueryResult.OnEach(el => {
                    var item = new TreeViewItem { Header = $"result {++counter}" };
                    el.OnEach(kv => item.Items.Add($"{kv.Key}: {kv.Value}"));
                    viewQuery.Items.Add(item);
                });

                var mess = this.QueryResult.Any() ? "Pomyślnie wykonano zapytanie." : "Zapytanie nie zwróciło żadnych wyników.";
                MessageBox.Show(mess, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var mess = (ex is DbException) ? $"Pojawił się błąd w realizacji zapytania:\n{ex.Message}" : ex.Message;
                MessageBox.Show(mess, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
