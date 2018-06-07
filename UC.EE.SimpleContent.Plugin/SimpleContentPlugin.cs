using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniconta.API.Plugin;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.Common;

namespace UC.EE.SimpleContent.Plugin
{
    public class SimpleContentPlugin : IContentPluginBase
    {
        private static RequestDialogForm content;
        CrudAPI crudApi;
        public string Name => "SimpleContentPlugin";
        public string errorDescription;

        public event EventHandler OnExecute;

        public ErrorCodes Execute(UnicontaBaseEntity master, UnicontaBaseEntity currentRow, IEnumerable<UnicontaBaseEntity> source, string command, string args)
        {
            // This will occur, when someone executes plugin.
            return ErrorCodes.Succes;
        }

        public string[] GetDependentAssembliesName()
        {
            return new string[] { "Newtonsoft.Json.dll" };
        }

        public string GetErrorDescription()
        {
            return errorDescription;
        }

        public void Intialize()
        {
            
        }

        public void OnPageClose()
        {
            // Something to do when page closes
        }

        public void SetAPI(BaseAPI api)
        {
            crudApi = api as CrudAPI;
        }

        public void SetContent(System.Windows.Controls.ContentControl control)
        {
            SimpleContentPlugin.content = new RequestDialogForm(this.crudApi);
            control.Content = SimpleContentPlugin.content;
        }

        public void SetMaster(List<UnicontaBaseEntity> masters)
        {
            
        }
    }
}
