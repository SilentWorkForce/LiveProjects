using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ConstructionNew.Enums
{
    public enum WorkType
    {
        //built an unselected
        [Description("Unselected")] Unselected = 0,
        [Description("Lead Man")] LeadMan,
        [Description("Foreman")] Foreman,
        [Description("Experienced MBA")] ExpMBA,
        [Description("New MBA")] NewMBA
    }

}