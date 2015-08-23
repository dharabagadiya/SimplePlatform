
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataModel.Modal
{
    public class ChartSeries
    {
        public class DataPoint
        {
            public int weekNumber;
            public double x;
            public double y;
        }
        public string type { get; set; }
        public string name { get; set; }
        public List<DataPoint> data { get; set; }
    }
}
