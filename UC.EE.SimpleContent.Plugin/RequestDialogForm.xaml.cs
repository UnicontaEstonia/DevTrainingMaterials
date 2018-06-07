using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uniconta.API.System;
using Uniconta.ClientTools.DataModel;
using Newtonsoft.Json;

namespace UC.EE.SimpleContent.Plugin
{
    /// <summary>
    /// Interaction logic for RequestDialogForm.xaml
    /// </summary>
    public partial class RequestDialogForm : UserControl
    {
        public RequestDialogForm()
        {
            InitializeComponent();
        }

        CreditorClient[] creditors;
        CrudAPI _crudApi;
        public RequestDialogForm(CrudAPI crudApi) : this()
        {
            _crudApi = crudApi;
            creditors = _crudApi.Query<CreditorClient>().Result;
            dgCreditors.ItemsSource = creditors;

            btnSearch.Click += BtnSearch_Click;
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            // It is free request of company full name and registration number from Estonian business registry
            var result = await wc.DownloadStringTaskAsync(new Uri("https://ariregister.rik.ee/autocomplete.py?nimi=" + txtOrganizationName.Text));
            var businessregistryObjects = JsonConvert.DeserializeObject<BusinessObject[]>(result);
            dgRegistryObjects.ItemsSource = businessregistryObjects;
        }

        private void ChooseOrganization(object sender, RoutedEventArgs e)
        {
            BusinessObject sendObj = ((Button)sender).DataContext as BusinessObject;
            MessageBox.Show("You have selected " + sendObj.nimi);
        }
    }
}
