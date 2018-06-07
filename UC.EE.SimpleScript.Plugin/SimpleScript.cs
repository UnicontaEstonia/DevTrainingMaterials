using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniconta.API.System;
using Uniconta.Common;
using Uniconta.DataModel;
using Uniconta.ClientTools.DataModel;
using Uniconta.API.Plugin;
using System.ComponentModel;

namespace UC.EE.SimpleScript.Plugin
{
    public class SimpleScript : PageEventsBase
    {
        public SimpleScript() : base()
        {
        }

        public override void Init(object page, CrudAPI api, UnicontaBaseEntity master)
        {
            base.Init(page, api, master);
        }

        public override ErrorCodes Validate(UnicontaBaseEntity record)
        {
            //ShowMessageBox("Validate called", "Validate");
            return 0;
        }

        public override ErrorCodes OnInsert(UnicontaBaseEntity record)
        {
            //ShowMessageBox("OnInsert called", "OnInsert");
            return 0;
        }

        public override ErrorCodes OnUpdate(UnicontaBaseEntity record, UnicontaBaseEntity original)
        {
            //ShowMessageBox("OnUpdate called", "OnUpdate");
            return 0;
        }

        public override ErrorCodes OnDelete(UnicontaBaseEntity record)
        {
            //ShowMessageBox("OnDelete called", "OnDelete");
            return 0;
        }

        public override void Record_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Print property
            //Trace(e.PropertyName);
            //Trace(e.PropertyName);
            //ShowMessageBox(e.PropertyName, "Property name");
        }
        public override bool OnMenuItemClicked(string ActionType, object sender)
        {
            //ShowMessageBox(ActionType, "Action type");
            return true;
        }
        public override void PageClosing()
        {
            // Show message box
            // this.ShowMessageBox("Page Closing", "Information");
        }
        public override string CheckMandatoryFields(UnicontaBaseEntity record)
        {
            DebtorOrderClient order = (DebtorOrderClient)record;
            if (String.IsNullOrEmpty(order.YourRef))
                return "Your reference";
            if (String.IsNullOrEmpty(order.CustomsNo))
                return "Customs number";
            return null;
        }
    }

}
