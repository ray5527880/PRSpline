using System;
using System.Collections.Generic;
using System.Text;

namespace BF_FW.data
{
    public class ExtremumData
    {
        public Extremum[] arrExtremum;
        public struct Extremum
        {
            public string strName;
            public decimal MaxValue;
            public decimal MinValue;
            public decimal MaxTime;
            public decimal MinTime;
        }
    }
}
