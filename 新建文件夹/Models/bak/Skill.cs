using System;
using System.Collections.Generic;
using System.Text;

namespace DateExplorer.Core.Models_Reversed
{
    public class Skill: ModelBase
    {
        public Skill():base()
        {
            this.TableName = "skill";
            this.KeyField = "ID";
            InspectModel();
        }

        public Skill(int id)
        {
            this.TableName = "skill";
            this.KeyField = "ID";
            Load(id);
        }
    }
}
