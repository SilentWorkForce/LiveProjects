using ConstructionNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ConstructionNew.Extensions
{
    public class WorkTypeMethods
    {
        public static List<string> GetWorkTypeDescription()
        {
            List<string> workList = new List<string>();
            foreach (WorkType workType in Enum.GetValues(typeof(WorkType)))
            {
                workList.Add(workType.GetDescription());
            }
            return workList;
        }


    }
}