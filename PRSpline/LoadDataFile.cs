using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace PRSpline
{
    class LoadDataFile
    {        
        public static List<DATData> ListDatData=new List<DATData>();
        //public string fileName = @"D:\AQ\AQ DR\2016-08-21_16-49-57-685_GCB5\2016-08-21_16-49-57-685_GCB5.dat";
        public void DisplayValues_CFG(string fileName,ref CFGData m_CFGData)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open)))
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
                                    m_CFGData.Location = value;
                                else if (index == 1)
                                    m_CFGData.Device = value;
                                index++;
                            }
                        }
                        else if (count == 1)
                        {
                            foreach (string value in ReadLine.Split(','))
                            {
                                if (value.IndexOf("A") != -1)
                                {
                                    m_CFGData.A_Amount = Convert.ToInt32(value.Substring(0, value.Length - 1));
                                    m_CFGData.arrAnalogyData = new CFGData.AnalogyData[m_CFGData.A_Amount];
                                }
                                else if (value.IndexOf("D") != -1)
                                {
                                    m_CFGData.D_Amount = Convert.ToInt32(value.Substring(0, value.Length - 1));
                                    m_CFGData.arrDigitalData = new CFGData.DigitalData[m_CFGData.D_Amount];
                                }
                                else
                                {
                                    m_CFGData.TotalAmount = Convert.ToInt32(value);
                                }
                            }
                        }
                        else if (count >= 2 && count < 2 + m_CFGData.TotalAmount)
                        {
                            if (ACount < m_CFGData.A_Amount)
                            {
                                string[] value = ReadLine.Split(',');
                                m_CFGData.arrAnalogyData[ACount].No = Convert.ToInt32(value[0]);
                                m_CFGData.arrAnalogyData[ACount].Name = value[1];
                                m_CFGData.arrAnalogyData[ACount].value1 = value[2];
                                m_CFGData.arrAnalogyData[ACount].value3 = value[3];
                                m_CFGData.arrAnalogyData[ACount].Unit = value[4];
                                m_CFGData.arrAnalogyData[ACount].Zoom = Convert.ToDecimal(value[5]);
                                m_CFGData.arrAnalogyData[ACount].value4 = Convert.ToDecimal(value[6]);
                                m_CFGData.arrAnalogyData[ACount].value5 = Convert.ToDecimal(value[7]);
                                m_CFGData.arrAnalogyData[ACount].value6 = Convert.ToDecimal(value[8]);
                                m_CFGData.arrAnalogyData[ACount].value7 = Convert.ToDecimal(value[9]);
                                m_CFGData.arrAnalogyData[ACount].Magnification1 = Convert.ToDecimal(value[value.Length - 3]);
                                m_CFGData.arrAnalogyData[ACount].Magnification2 = Convert.ToDecimal(value[value.Length - 2]);
                                m_CFGData.arrAnalogyData[ACount].PrimaryOrSecondary = value[value.Length - 1];
                                ACount++;
                            }
                            else if (DCount < m_CFGData.D_Amount)
                            {
                                string[] value = ReadLine.Split(',');
                                m_CFGData.arrDigitalData[DCount].No = Convert.ToInt32(value[0]);
                                m_CFGData.arrDigitalData[DCount].Name = value[1];
                                m_CFGData.arrDigitalData[DCount].value1 = value[2];
                                m_CFGData.arrDigitalData[DCount].value2 = value[3];
                                m_CFGData.arrDigitalData[DCount].value3 = value[4];
                                DCount++;
                            }
                        }
                        else if (count == 2 + m_CFGData.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            m_CFGData.Hz = Convert.ToDecimal(value[0]);
                        }
                        else if (count == 4 + m_CFGData.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            m_CFGData.SamplingPoint = Convert.ToDecimal(value[0]);
                            m_CFGData.TotalPoint = Convert.ToDecimal(value[1]);
                        }
                        else if (count == 5 + m_CFGData.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            m_CFGData.startDate = value[0];
                            m_CFGData.startTime = value[1];
                        }
                        else if (count == 6 + m_CFGData.TotalAmount)
                        {
                            string[] value = ReadLine.Split(',');
                            m_CFGData.triggerDate = value[0];
                            m_CFGData.triggerTime = value[1];
                        }
                        count++;
                    }

                }
            }
            else
            {
                throw new ApplicationException("檔案不存在");
            }
        }
        public void DisplayValues_DAT(string fileName,ref DATData m_DATData,int ValueCount)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open)))
                {
                    m_DATData.arrData = new List<DATData.Data>();
                    foreach (var ReadLine in reader.ReadToEnd().Split('\n'))
                    {
                        int count = 0;

                        DATData.Data n_Data = new DATData.Data();
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
                        if (ReadLine.Split(',').Length < 2) return;
                        m_DATData.arrData.Add(n_Data);
                    }

                }
            }
        }
    }
}
