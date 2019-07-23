using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConstructionNew.Enums
{
    public enum State
    {
        [Description("Oregon"), Display(Name = "Oregon")]
        OR,
        [Description("Washington"), Display(Name = "Washington")]
        WA,
        [Description("California"), Display(Name = "California")]
        CA
    }
}