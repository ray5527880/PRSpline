using System;
using System.Collections.Generic;
using System.Text;
using GSF.COMTRADE;
using System.IO;
using BF_FW.data;
using FFTWSharp;
using System.Numerics;

namespace BF_FW
{
    public class VoltageSagCal
    {
  
        public int BaseVolateg;
        public VoltageSagData.voltageSagData VoltageSagDatas;

        double douMinTime;
        public struct VSagValue
        {
            public bool Is_Start;
            public bool IsVSag;
            public double MinVoltage;
        }
        public struct VSag
        {
            public double douStartTime;
            public double douEndTime;
            public VSagValue[] vSagValues;
        }
        private int[] VIndex = new int[3];
        private VSag stuVSag = new VSag();
        private VSag stuVSag2 = new VSag();
        Parser mParser = new Parser();
        List<double[]> PData;
        List<double[]> SData;
        List<double[]> PUData;
        FFTData mFFTData = new FFTData();

        private bool IsStop = false;

        public VoltageSagCal(string filepath, int basevoltage)
        {
            VoltageSagDatas = new VoltageSagData.voltageSagData();
            BaseVolateg = basevoltage;
            string strFileName = filepath;
            string strFolderPath = string.Empty;
            string strUnFilePath = string.Empty;

            while (strFileName.IndexOf(@"\") > -1)
            {
                strFolderPath += strFileName.Substring(0, strFileName.IndexOf(@"\") + 1);
                strFileName = strFileName.Substring(strFileName.IndexOf(@"\") + 1);
            }
            strUnFilePath = strFolderPath + @"../.././CompressFile";
            //var tttt = Path.GetFullPath(strUnFilePath);
            if (Directory.Exists(strUnFilePath))
            {
                Directory.Delete(strUnFilePath, true);
            }

            CompressWinRAR compressWinRAR = new CompressWinRAR();

            compressWinRAR.UnCompressRar(Path.GetFullPath(strUnFilePath), strFolderPath, strFileName);

            foreach (var item in Directory.GetFiles(Path.GetFullPath(strUnFilePath), "*.cfg"))
            {
                LoadDataFile.GetCFGData(item,ref mParser);
                break;
            }
            foreach (var item in Directory.GetFiles(strUnFilePath, "*.CFG"))
            {
                LoadDataFile.GetCFGData(item,ref mParser);
                break;
            }

            PData = new List<double[]>();
            SData = new List<double[]>();
            PUData = new List<double[]>();
            LoadDataFile.GetDatData(mParser, ref PData, ref SData, ref PUData);

            douMinTime = 1 / mParser.Schema.NominalFrequency / 2;

            for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
            {
                if (mParser.Schema.AnalogChannels[i].Units == "V")
                {
                    if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "A")
                        VIndex[0] = i;
                    else if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "B")
                        VIndex[1] = i;
                    else if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "C")
                        VIndex[2] = i;
                }
            }
            var mfft = new FFTCal(VIndex, mParser);
            mFFTData = mfft.GetFFTData(PData);

           
            try
            {
                VolSagCal();
                if (stuVSag2.douEndTime == 0)
                    VolSagCal_95();
                SetVolSagData();
                mParser.CloseFiles();
                mParser.Dispose();
            }
            catch(Exception eee) 
            {

            }
        }
        private void SetVolSagData()
        {
           
            if (stuVSag2.douEndTime != 0)
            {
                VoltageSagDatas.treggerDateTime = Convert.ToDateTime(mParser.Schema.TriggerTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                VoltageSagDatas.duration = Math.Round(Convert.ToDecimal(stuVSag2.douEndTime - stuVSag2.douStartTime), 1);
                VoltageSagDatas.StartTime = Math.Round(Convert.ToDecimal(stuVSag2.douStartTime), 1);
                VoltageSagDatas.cycle = Math.Round(Convert.ToDecimal((stuVSag2.douEndTime - stuVSag2.douStartTime) * mParser.Schema.NominalFrequency)/1000, 1);
                VoltageSagDatas.PValue = Math.Round(Convert.ToDecimal(stuVSag2.vSagValues[0].MinVoltage), 2);
                VoltageSagDatas.QValue = Math.Round(Convert.ToDecimal(stuVSag2.vSagValues[1].MinVoltage), 2);
                VoltageSagDatas.SValue = Math.Round(Convert.ToDecimal(stuVSag2.vSagValues[2].MinVoltage), 2);

            }
        }
        private VSag ResetVSagValue()
        {
            var _VSag = new VSag();
            _VSag.douStartTime = 0;
            _VSag.douEndTime = 0;
            _VSag.vSagValues = new VSagValue[3];
            for (int i = 0; i < 3; i++)
            {
                _VSag.vSagValues[i].IsVSag = false;
                _VSag.vSagValues[i].MinVoltage = 0;
                _VSag.vSagValues[i].Is_Start = true;
            }
            return _VSag;
        }

        private void VolSagCal()
        {
            stuVSag = ResetVSagValue();
            stuVSag2 = ResetVSagValue();
            int halfCyclePoint = Convert.ToInt32(mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency) / 2;
            for (int i = halfCyclePoint; i < mParser.Schema.SampleRates[0].EndSample - halfCyclePoint; i++)
            {
                if (!stuVSag.vSagValues[0].IsVSag && !stuVSag.vSagValues[1].IsVSag && !stuVSag.vSagValues[2].IsVSag)
                {
                    //PData[i][VIndex[ii] + 2] / BaseVolateg
                    for (int ii = 0; ii < VIndex.Length; ii++)
                    {
                        if (!stuVSag.vSagValues[ii].Is_Start && mFFTData.arrFFTData[i].Value[ii] / BaseVolateg > 0.9f)
                            stuVSag.vSagValues[ii].Is_Start = true;
                        if (mFFTData.arrFFTData[i].Value[ii] / BaseVolateg < 0.9f && mFFTData.arrFFTData[i].Value[ii] / BaseVolateg > 0.1f && stuVSag.vSagValues[ii].Is_Start)
                        {
                            stuVSag.vSagValues[ii].IsVSag = true;
                            stuVSag.douStartTime = PUData[i][1];
                        }
                    }
                }
                else
                {
                    if (IsStop)
                        return;
                    if (stuVSag.vSagValues[0].IsVSag)
                    {
                        VolSagDefinition(0, mFFTData.arrFFTData[i], PData[i]);
                    }
                    else if (stuVSag.vSagValues[1].IsVSag)
                    {
                        VolSagDefinition(1, mFFTData.arrFFTData[i], PData[i]);
                    }
                    else if (stuVSag.vSagValues[2].IsVSag)
                    {
                        VolSagDefinition(2, mFFTData.arrFFTData[i], PData[i]);
                    }
                }

            }
        }


        private void VolSagDefinition(int FFTIndex, FFTData.fftData fftData, double[] puData)
        {
            if (fftData.Value[FFTIndex] / BaseVolateg > 0.9f)
            {
                //if (puData[1] - stuVSag.douStartTime > douMinTime)
                //{
                stuVSag.douEndTime = puData[1];
                if ((stuVSag.douEndTime - stuVSag.douStartTime) > (stuVSag2.douEndTime - stuVSag2.douStartTime))
                    stuVSag2 = stuVSag;
                //}
                stuVSag = ResetVSagValue();
            }
            else if (fftData.Value[FFTIndex] / BaseVolateg < 0.1f)
            {
                stuVSag = ResetVSagValue();
                IsStop = true;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (stuVSag.vSagValues[i].MinVoltage > fftData.Value[i] / BaseVolateg || stuVSag.vSagValues[i].MinVoltage == 0)
                    {
                        stuVSag.vSagValues[i].MinVoltage = fftData.Value[i] / BaseVolateg;
                    }
                }
            }
        }
        private void VolSagCal_95()
        {
            stuVSag = ResetVSagValue();
            stuVSag2 = ResetVSagValue();
            int halfCyclePoint = Convert.ToInt32(mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency) / 2;
            for (int i = halfCyclePoint; i < mParser.Schema.SampleRates[0].EndSample - halfCyclePoint; i++)
            {
                if (!stuVSag.vSagValues[0].IsVSag && !stuVSag.vSagValues[1].IsVSag && !stuVSag.vSagValues[2].IsVSag)
                {
                    //PData[i][VIndex[ii] + 2] / BaseVolateg
                    for (int ii = 0; ii < VIndex.Length; ii++)
                    {
                        if (!stuVSag.vSagValues[ii].Is_Start && mFFTData.arrFFTData[i].Value[ii] / BaseVolateg > 0.95f)
                            stuVSag.vSagValues[ii].Is_Start = true;
                        if (mFFTData.arrFFTData[i].Value[ii] / BaseVolateg < 0.95f && mFFTData.arrFFTData[i].Value[ii] / BaseVolateg > 0.10f && stuVSag.vSagValues[ii].Is_Start)
                        {
                            stuVSag.vSagValues[ii].IsVSag = true;
                            stuVSag.douStartTime = PUData[i][1];
                        }
                    }
                }
                else
                {
                    if (IsStop)
                        return;
                    if (stuVSag.vSagValues[0].IsVSag)
                    {
                        VolSagDefinition_95(0, mFFTData.arrFFTData[i], PData[i]);
                    }
                    else if (stuVSag.vSagValues[1].IsVSag)
                    {
                        VolSagDefinition_95(1, mFFTData.arrFFTData[i], PData[i]);
                    }
                    else if (stuVSag.vSagValues[2].IsVSag)
                    {
                        VolSagDefinition_95(2, mFFTData.arrFFTData[i], PData[i]);
                    }
                }

            }
        }


        private void VolSagDefinition_95(int FFTIndex, FFTData.fftData fftData, double[] puData)
        {
            if (fftData.Value[FFTIndex] / BaseVolateg > 0.95f)
            {
                //if (puData[1] - stuVSag.douStartTime > douMinTime)
                //{
                stuVSag.douEndTime = puData[1];
                if ((stuVSag.douEndTime - stuVSag.douStartTime) > (stuVSag2.douEndTime - stuVSag2.douStartTime))
                    stuVSag2 = stuVSag;
                //}
                stuVSag = ResetVSagValue();
            }
            else if (fftData.Value[FFTIndex] / BaseVolateg < 0.1f)
            {
                stuVSag = ResetVSagValue();
                IsStop = true;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (stuVSag.vSagValues[i].MinVoltage > fftData.Value[i] / BaseVolateg || stuVSag.vSagValues[i].MinVoltage == 0)
                    {
                        stuVSag.vSagValues[i].MinVoltage = fftData.Value[i] / BaseVolateg;
                    }
                }
            }
        }
    }
}
