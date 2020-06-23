using System;
using System.Collections.Generic;
using System.Text;

namespace BF_FW.data
{
    public class FFTData
    {
        public fftData[] arrFFTData;
        public struct fftData
        {
            public double[] Value;
            public double[] rad;
        }
    }
}
