using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRSpline
{
    public class DATData
    {
        public List<Data> arrData;
        public struct Data
        {
            public int No;
            public decimal Time;
            public decimal[] value;
        }
    }
}
