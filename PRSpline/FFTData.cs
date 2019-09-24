using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRSpline
{
    public class FFTData
    {
        public List<Data> arrFFTData;
        public struct Data
        {
            public double[] Value;
            public double[] rad;
        }
    }
}
