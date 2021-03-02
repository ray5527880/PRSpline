using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GSF;
using GSF.COMTRADE;

namespace BF_FW.data
{
    public class LoadDataFile
    {
        public static void GetCFGData(string filePath,ref Parser parser)
        {
            parser = new Parser();
            parser.Schema = new Schema(filePath);

            parser.InferTimeFromSampleRates = true;
            parser.OpenFiles();
            //return parser;
        }
      

        public static void GetDatData(Parser parser, ref List<double[]> PrimaryData, ref List<double[]> SecondaryData, ref List<double[]> PerUnitData)
        {
            int index = 0;
            var data = new List<double[]>();
            double[] vs = new double[parser.Schema.TotalChannels + 2];
            while (parser.ReadNext())
            {
                double[] PData = new double[parser.Schema.TotalChannels + 2];
                double[] SData = new double[parser.Schema.TotalChannels + 2];
                double[] PUData = new double[parser.Schema.TotalChannels + 2];

                PData[0] = index;
                SData[0] = index;
                PUData[0] = index;

                PData[1] = TimeSpan.FromTicks(parser.Timestamp.Ticks - parser.Schema.StartTime.Value).TotalMilliseconds;
                SData[1] = TimeSpan.FromTicks(parser.Timestamp.Ticks - parser.Schema.StartTime.Value).TotalMilliseconds;
                PUData[1] = TimeSpan.FromTicks(parser.Timestamp.Ticks - parser.Schema.StartTime.Value).TotalMilliseconds;

                for (int i = 0; i < parser.Schema.TotalAnalogChannels; i++)
                {
                    PData[i + 2] = parser.PrimaryValues[i];
                }

                for (int i = 0; i < parser.Schema.TotalAnalogChannels; i++)
                {
                    SData[i + 2] = parser.SecondaryValues[i];
                }

                for (int i = 0; i < parser.Schema.TotalAnalogChannels; i++)
                {
                    if (parser.Schema.AnalogChannels[i].ScalingIdentifier == 'S')
                        PUData[i + 2] = parser.SecondaryValues[i] / parser.Schema.AnalogChannels[i].SecondaryRatio;
                    else if (parser.Schema.AnalogChannels[i].ScalingIdentifier == 'P')
                        PUData[i + 2] = parser.PrimaryValues[i] / parser.Schema.AnalogChannels[i].PrimaryRatio;
                }
                for (int i = 0; i < parser.Schema.TotalDigitalChannels; i++)
                {
                    PUData[i + 2 + parser.Schema.TotalAnalogChannels] = parser.Values[parser.Schema.TotalAnalogChannels + i];
                }

                PrimaryData.Add(PData);
                SecondaryData.Add(SData);
                PerUnitData.Add(PUData);
                index++;
            }
            parser.Dispose();
            parser.CloseFiles();
        }
    }
}
