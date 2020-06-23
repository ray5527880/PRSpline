using System;
using System.Collections.Generic;
using System.Text;
using BF_FW.data;

namespace BF_FW
{
    public class ExtremumFunction
    {
        public static ExtremumData.Extremum[] GetExtremunData(DATData.datData[] datas,CFGData cFGData,decimal[] ps)
        {

            int count = cFGData.A_Amount;
            
            var _data = new ExtremumData.Extremum[count];

            foreach(var item in datas)
            {
                for(int i=0;i< count; i++)
                {
                    if (_data[i].MaxValue < item.value[i])
                    {
                        _data[i].MaxValue = item.value[i];
                        _data[i].MaxTime = item.Time;
                    }
                    if (_data[i].MinValue > item.value[i])
                    {
                        _data[i].MinValue = item.value[i];
                        _data[i].MinTime = item.Time;
                    }
                }
            }

            for(int i = 0; i < count; i++)
            {
                _data[i].strName = cFGData.arrAnalogyData[i].Name;
                _data[i].MaxTime = _data[i].MaxTime / 1000;
                _data[i].MinTime = _data[i].MinTime / 1000;
                _data[i].MaxValue = _data[i].MaxValue * ps[i];
                _data[i].MinValue = _data[i].MinValue * ps[i];
            }
            

            return _data;
        }
    }
}
