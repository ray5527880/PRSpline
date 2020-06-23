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
        //public static Parser Get_CFGData(string filePath)
        //{
        //    Parser parser = new Parser();



        //    return parser;
        //}
        //public static List<double[]> GetDatData(Parser parser)
        //{
        //    var data = new List<double[]>();

        //    return data;
        //}


        public static CFGData Get_CFGData(string filePath)
        {
            var _data = new CFGData();

            Parser parser = new Parser();
            parser.Schema = new Schema(filePath);
            //parser.Schema.
            //parser.FileName = filePath;
            parser.InferTimeFromSampleRates = true;
            parser.OpenFiles();
            var dates = new List<double[]>();
            while(parser.ReadNext())
            {
                dates.Add(parser.Values);
            }    
            


            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open)))
                {
                    int count = 0;
                    int ACount = 0;
                    int DCount = 0;
                    foreach (var ReadLine in reader.ReadToEnd().Split('\n'))
                    {
                        if (count == 0)
                        {
                            int index = 0;
                            foreach (string value in ReadLine.Split(','))
                            {
                                if (index == 0)
                                    _data.Location = value;
                                else if (index == 1)
                                    _data.Device = value;
                                index++;
                            }
                        }
                        else if (count == 1)
                        {
                            foreach (string value in ReadLine.Split(','))
                            {
                                if (value.IndexOf("A") != -1)
                                {
                                    _data.A_Amount = Convert.ToInt32(value.Substring(0, value.Length - 1));
                                    _data.arrAnalogyData = new CFGData.AnalogyData[_data.A_Amount];
                                }
                                else if (value.IndexOf("D") != -1)
                                {
                                    _data.D_Amount = Convert.ToInt32(value.Substring(0, value.Length - 1));
                                    _data.arrDigitalData = new CFGData.DigitalData[_data.D_Amount];
                                }
                                else
                                {
                                    _data.TotalAmount = Convert.ToInt32(value);
                                }
                            }
                        }
                        else if (count >= 2 && count < 2 + _data.TotalAmount)
                        {
                            if (ACount < _data.A_Amount)
                            {
                                string[] value = ReadLine.Split(',');
                                _data.arrAnalogyData[ACount].No = Convert.ToInt32(value[0]);
                                _data.arrAnalogyData[ACount].Name = value[1];
                                _data.arrAnalogyData[ACount].Phase = value[2];
                                _data.arrAnalogyData[ACount].value3 = value[3];
                                _data.arrAnalogyData[ACount].Unit = value[4];
                                _data.arrAnalogyData[ACount].Zoom = Convert.ToDecimal(value[5]);
                                _data.arrAnalogyData[ACount].value4 = Convert.ToDecimal(value[6]);
                                _data.arrAnalogyData[ACount].value5 = Convert.ToDecimal(value[7]);
                                _data.arrAnalogyData[ACount].value6 = Convert.ToDecimal(value[8]);
                                _data.arrAnalogyData[ACount].value7 = Convert.ToDecimal(value[9]);
                                _data.arrAnalogyData[ACount].Magnification1 = Convert.ToDecimal(value[value.Length - 3]);
                                _data.arrAnalogyData[ACount].Magnification2 = Convert.ToDecimal(value[value.Length - 2]);
                                _data.arrAnalogyData[ACount].PrimaryOrSecondary = value[value.Length - 1];
                                ACount++;
                            }
                            else if (DCount < _data.D_Amount)
                            {
                                string[] value = ReadLine.Split(',');
                                _data.arrDigitalData[DCount].No = Convert.ToInt32(value[0]);
                                _data.arrDigitalData[DCount].Name = value[1];
                                _data.arrDigitalData[DCount].value1 = value[2];
                                _data.arrDigitalData[DCount].value2 = value[3];
                                _data.arrDigitalData[DCount].value3 = value[4];
                                DCount++;
                            }
                        }
                        else if (count == 2 + _data.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            _data.Hz = Convert.ToDecimal(value[0]);
                        }
                        else if (count == 4 + _data.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            _data.SamplingPoint = Convert.ToDecimal(value[0]);
                            _data.TotalPoint = Convert.ToDecimal(value[1]);
                        }
                        else if (count == 5 + _data.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            _data.startDate = value[0];
                            _data.startTime = value[1];
                        }
                        else if (count == 6 + _data.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            _data.triggerDate = value[0];
                            _data.triggerTime = value[1];
                        }
                        else if (count == 7 + _data.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            _data.CodeType = value[0];
                        }
                        count++;
                    }

                }
            }
            else
            {
                throw new ApplicationException("檔案不存在");
            }
            return _data;
        }
        public static DATData Get_DATData(string filePath,int ValueCount)
        {
            var _data = new DATData();
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(File.Open(filePath, FileMode.Open)))
                {
                    var data = new List<DATData.datData>();
                    foreach (var ReadLine in reader.ReadToEnd().Split('\n'))
                    {
                        int count = 0;

                        DATData.datData n_Data = new DATData.datData();
                        n_Data.value = new decimal[ValueCount];
                        foreach (var value in ReadLine.Split(','))
                        {
                            if (ReadLine.Split(',').Length < 2) break;
                            if (count == 0)
                            {
                                n_Data.No = Convert.ToInt32(value);
                            }
                            else if (count == 1)
                            {
                                n_Data.Time = Convert.ToDecimal(value);
                            }
                            else
                            {
                                n_Data.value[count - 2] = Convert.ToDecimal(value);
                            }
                            count++;
                        }
                        if (ReadLine.Split(',').Length < 2) continue;
                        data.Add(n_Data);
                    }
                    _data.arrData = data.ToArray();

                }
            }

            return _data;
        }
       
    }
}
