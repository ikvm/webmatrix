using System;
using System.Collections.Generic;
using System.Text;

namespace DateExplorer.Core.Models_Reversed
{
    public class Buff: ModelBase
    {
        public Buff():base()
        {
            this.TableName = "buff";
            this.KeyField = "ID";
            InspectModel();
        }

        public Buff(int id)
        {
            this.TableName = "buff";
            this.KeyField = "ID";
            Load(id);
        }
    }
}
