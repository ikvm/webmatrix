using System;
using System.Collections.Generic;
using System.Text;

namespace DateExplorer.Core.Models_Reversed
{
    public class EffectI: ModelBase
    {
        public EffectI(): base()
        {
            this.TableName = "effect_i";
            this.KeyField = "ID";
            InspectModel();
        }
    }
}
