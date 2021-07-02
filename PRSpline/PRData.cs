using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.COMTRADE;

namespace PRSpline
{
    public class PRData
    {
        private List<string> ButtonName = new List<string>();
        public string strFileName;
        public Parser mParser;
        public List<double[]> PData;
        public List<double[]> SData;
        public List<double[]> PUData;
    }
}
