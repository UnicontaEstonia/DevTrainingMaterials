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
using Uniconta.ClientTools.Controls;
using Uniconta.ClientTools.DataModel;
using Uniconta.ClientTools.Page;

namespace UC.EE.SimpleDevExContent.Plugin
{
    /// <summary>
    /// Interaction logic for GridSampleForm.xaml
    /// </summary>
    public partial class GridSampleForm : GridBasePage
    {
        public GridSampleForm() : base(null)
        {
            InitializeComponent();

            carGrid.api = api;
            localMenu.dataGrid = carGrid;

            localMenu.AddRibbonItems(CreateRibbonItems());

            SetRibbonControl(localMenu, carGrid);

            localMenu.OnItemClicked += LocalMenu_OnItemClicked; // to add on-click function
            
            BindGrid();
        }

        private void BindGrid()
        {
            var t = carGrid.Filter(null);
            StartLoadCache(t);
        }

        private List<TreeRibbon> CreateRibbonItems()
        {
            var ribbonItems = new List<TreeRibbon>();
            var addRowItem = new TreeRibbon();
            addRowItem.ActionName = "AddRow";
            addRowItem.Name = "Add Row";
            addRowItem.LargeGlyph = LargeIcon.Add.ToString();

            var editRowItem = new TreeRibbon();
            editRowItem.ActionName = "EditRow";
            editRowItem.Name = "Edit Row";
            editRowItem.LargeGlyph = LargeIcon.Edit.ToString();

            var refreshRowItem = new TreeRibbon();
            refreshRowItem.ActionName = "RefreshGrid";
            refreshRowItem.Name = "Refresh";
            refreshRowItem.LargeGlyph = LargeIcon.Refresh.ToString();

            var exportToXml = new TreeRibbon();
            exportToXml.ActionName = "GenerateXml";
            exportToXml.Name = "Generate XML";
            exportToXml.LargeGlyph = LargeIcon.Generate.ToString();

            ribbonItems.Add(addRowItem);
            ribbonItems.Add(editRowItem);
            ribbonItems.Add(refreshRowItem);
            ribbonItems.Add(exportToXml);
            return ribbonItems;
        }

        private void LocalMenu_OnItemClicked(string ActionType)
        {
            var selectedItem = carGrid.SelectedItem as Cars;
            switch (ActionType)
            {
                case "AddRow":
                    object[] param = new object[2];
                    param[0] = api;
                    param[1] = null;
                    // Lisada parameetrina BindGrid event sisse, siis teeks refreshi peale salvestamist... Lihtsalt mõte
                    AddDockItem("PluginAddEditCars", param, "Add new car");
                    break;
                case "EditRow":
                    if (selectedItem != null)
                        AddDockItem("PluginAddEditCars", selectedItem, string.Format("{0}_{1}", "Car", selectedItem.Model));
                    break;
                case "RefreshGrid":
                    BindGrid();
                    break;
                case "GenerateXml":
                    // Generate or do some extra work
                    break;
                default:
                    // do something if no action is set
                    break;
            }
        }
    }

    public class CarGrid : UnicontaDataGrid
    {
        public override Type TableType
        {
            get { return typeof(Cars); }
        }
    }
}
