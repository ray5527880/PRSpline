using GSF.COMTRADE;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF_FW.data
{
    public   class ChartData
    {
        public string strFileName ;
        public Parser mParser;
        public List<double[]> PData;
        public List<double[]> SData;
        public List<double[]> PUData;
        public List<string> ButtonName;
        public ChartData()
        {
            strFileName = string.Empty;
            mParser = new Parser();
            PData = new List<double[]>();
            SData = new List<double[]>();
            PUData = new List<double[]>();
            ButtonName = new List<string>();
        }
    }
}
