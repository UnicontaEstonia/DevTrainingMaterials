using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.ClientTools;
using Uniconta.Common;

namespace UC.EE.SimpleDevExContent.Plugin
{
    public class SimpleDevExContent : IPluginControl
    {
        public string Name => "UC.EE.SimpleDevExContent.Plugin";
        string _errorDescription;
        CrudAPI _crudApi;

        public event EventHandler OnExecute;

        public ErrorCodes Execute(UnicontaBaseEntity master, UnicontaBaseEntity currentRow, IEnumerable<UnicontaBaseEntity> source, string command, string args)
        {
            return ErrorCodes.Succes;
        }

        public string[] GetDependentAssembliesName()
        {
            return null;
        }

        public string GetErrorDescription()
        {
            return _errorDescription;
        }

        public void Intialize()
        {
            
        }

        public List<PluginControl> RegisterControls()
        {
            var ctrls = new List<PluginControl>();
            ctrls.Add(new PluginControl() { UniqueName = "GridSampleForm", PageType = typeof(GridSampleForm), AllowMultipleOpen = false, PageHeader = "Grid sample form" });
            ctrls.Add(new PluginControl() { UniqueName = "PluginAddEditCars", PageType = typeof(FormPageSampleForm), AllowMultipleOpen = false, PageHeader = "Form page sample" });
            return ctrls;
        }

        public void SetAPI(BaseAPI api)
        {
            _crudApi = api as CrudAPI;
        }

        public void SetMaster(List<UnicontaBaseEntity> masters)
        {
            
        }
    }
}
