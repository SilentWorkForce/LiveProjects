using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ManagementPortal.Enums
{
    public enum WorkType
    {
        //Added an unselected
        [Description("Unselected")] Unselected = 0,
        [Description("Lead Man")] Leadman ,
        [Description("Foreman")] Foreman,
        [Description("Experienced MBA")] ExpMBA ,
        [Description("New MBA")] NewMBA 

    }
}