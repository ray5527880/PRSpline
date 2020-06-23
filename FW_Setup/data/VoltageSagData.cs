using System;
using System.Collections.Generic;
using System.Text;

namespace BF_FW.data
{
    public class VoltageSagData
    {
        public voltageSagData[] DataArray;
        public struct voltageSagData
        {
            public DateTime treggerDateTime;
            public decimal duration;
            public decimal PValue;
            public decimal QValue;
            public decimal SValue;
        }
    }
}
