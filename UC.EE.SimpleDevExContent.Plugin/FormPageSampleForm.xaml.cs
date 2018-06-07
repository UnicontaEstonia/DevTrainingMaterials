using System;
using System.Collections.Generic;
using System.Linq;
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
using UC.EE.SimpleDevExContent.Plugin.Model;
using Uniconta.API.System;
using Uniconta.ClientTools.Controls;
using Uniconta.ClientTools.Page;
using Uniconta.Common;

namespace UC.EE.SimpleDevExContent.Plugin
{
    /// <summary>
    /// Interaction logic for FormPageSampleForm.xaml
    /// </summary>
    public partial class FormPageSampleForm : FormBasePage
    {
        private Cars editrow;

        /* for edit*/
        public FormPageSampleForm(UnicontaBaseEntity sourcedata) : base(sourcedata, true)
        {
            InitializeComponent();
            InitPage(api);
        }
        /* for add */
        public FormPageSampleForm(CrudAPI crudApi, string dummy) : base(crudApi, dummy)
        {
            InitializeComponent();
            InitPage(crudApi);
        }

        void InitPage(CrudAPI crudapi)
        {
            frmRibbon.AddRibbonItems(CreateRibbonItems());
            ribbonControl = frmRibbon;
            layoutControl = layoutItems;
            if (LoadedRow == null)
                editrow = CreateNew() as Cars;
            layoutItems.DataContext = editrow;
            typelookupeditor.api = api; /* api should be set of all lookupEditors used in form */
            typelookupeditor.ItemsSource = new[] { "Sedan", "Universal", "Hatchback" };
            frmRibbon.OnItemClicked += frmRibbon_OnItemClicked;

        }

        private void frmRibbon_OnItemClicked(string ActionType)
        {
            switch (ActionType)
            {
                default:
                    frmRibbon_BaseActions(ActionType);
                    break;
            }
        }

        private List<TreeRibbon> CreateRibbonItems()
        {
            var ribbonItems = new List<TreeRibbon>();
            var saveItem = new TreeRibbon();
            saveItem.ActionName = "Save";
            saveItem.Name = "Save";
            saveItem.LargeGlyph = LargeIcon.Save.ToString();

            ribbonItems.Add(saveItem);
            return ribbonItems;
        }

        public override UnicontaBaseEntity ModifiedRow { get { return editrow; } set { editrow = (Cars)value; } }

        public override string NameOfControl { get { return "PluginAddEditCars"; } }

        public override Type TableType { get { return typeof(Cars); } }

        public override void OnClosePage(object[] refreshParams)
        {

        }
    }
}
