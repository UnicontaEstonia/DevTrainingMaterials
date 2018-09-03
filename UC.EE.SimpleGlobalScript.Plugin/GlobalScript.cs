using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniconta.API.Plugin;
using Uniconta.API.System;
using Uniconta.Common;

namespace UC.EE.SimpleGlobalScript.Plugin
{
    public class GlobalScript : PageEventsBase
    {
        public GlobalScript()
        { }

        public override void Init(object page, CrudAPI api, UnicontaBaseEntity master)
        {
            base.Init(page, api, master);
            Trace("Company changed");
        }

        public override bool OnMenuItemClicked(string ActionType, object sender)
        {
            Trace("Action: " + ActionType);
            return base.OnMenuItemClicked(ActionType, sender);
        }
    }
}
