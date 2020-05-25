using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRSpline
{
    public class CFGData
    {
        public string Location;
        public string Device;
        public int TotalAmount;
        public int A_Amount;
        public int D_Amount;
        public string startDate;
        public string startTime;
        public string triggerDate;
        public string triggerTime;
        public decimal Hz;
        public decimal SamplingPoint;
        public decimal TotalPoint;
        public AnalogyData[] arrAnalogyData;
        public struct AnalogyData
        {
            public int No;
            public string Name;
            public string value1;
            public string Unit;
            public string value3;
            public decimal value4;
            public decimal value5;
            public decimal value6;
            public decimal value7;
            public decimal Zoom;
            public decimal Magnification1;
            public decimal Magnification2;
            public string PrimaryOrSecondary;
        }
        public DigitalData[] arrDigitalData;
        public struct DigitalData
        {
            public int No;
            public string Name;
            public string value1;
            public string value2;
            public string value3;
        }
    }
}
