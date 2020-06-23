using System;
using System.Collections.Generic;
using System.Text;

namespace BF_FW.data
{
    public class DATData
    {
        public datData[] arrData;
        public struct datData
        {
            public int No;
            public decimal Time;
            public decimal[] value;
        }
    }
}
