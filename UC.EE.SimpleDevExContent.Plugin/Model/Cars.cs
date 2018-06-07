using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniconta.DataModel;

namespace UC.EE.SimpleDevExContent.Plugin.Model
{

    public class Cars : TableDataWithKey
    {
        public override int UserDefinedId { get { return 1005; } }
        public override Type MasterType { get { return typeof(Uniconta.DataModel.Debtor); } }

        public string Mark
        {
            get { return this.GetUserFieldString(0); }
            set { this.SetUserFieldString(0, value); }
        }

        public string Model
        {
            get { return this.GetUserFieldString(1); }
            set { this.SetUserFieldString(1, value); }
        }

        public long Year
        {
            get { return this.GetUserFieldInt64(2); }
            set { this.SetUserFieldInt64(2, value); }
        }

        public string Type
        {
            get { return this.GetUserFieldString(3); }
            set { this.SetUserFieldString(3, value); }
        }

        public DateTime FirstRegistration
        {
            get { return this.GetUserFieldDateTime(4); }
            set { this.SetUserFieldDateTime(4, value); }
        }
    }


}
