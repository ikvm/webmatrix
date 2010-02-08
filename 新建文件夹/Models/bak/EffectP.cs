using System;
using System.Collections.Generic;
using System.Text;

namespace DateExplorer.Core.Models_Reversed
{
    public class EffectP: ModelBase
    {
        public EffectP() : base()
        {
            this.TableName = "effect_p";
            this.KeyField = "ID";
            InspectModel();
        }
    }
}
