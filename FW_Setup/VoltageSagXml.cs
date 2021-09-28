using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using BF_FW.data;

namespace BF_FW
{
    public class VoltageSagXml
    {
        string _filePaht;
        public VoltageSagXml(string filePath)
        {
            _filePaht = filePath;
        }
        public bool CreateFile()
        {
            bool IsCreate = false;
            //File.Create(_filePaht);
            
            try
            {
                using (XmlWriter writer = XmlWriter.Create(_filePaht))
                {
                    writer.WriteStartElement("root");                  
                    writer.WriteEndElement();
                    writer.Flush();
                }
                IsCreate = true;
            }
            catch (Exception eee) { }
            return IsCreate;
        }
        public void AddData(VoltageSagData.voltageSagData data)
        {
            var OldData = GetXmlData();
            var NewData = new List<VoltageSagData.voltageSagData>();
            foreach (var item in OldData)
            {
                NewData.Add(item);
            }
            NewData.Add(data);
            SaveXml(NewData.ToArray());
        }
        public void Updata(string  dateTime,string strPSvalue)
        {
            var OldData = GetXmlData();
            var NewData = new List<VoltageSagData.voltageSagData>();
            foreach (var item in OldData)
            {
                if (item.treggerDateTime.ToString("yyyy-MM-dd_HH-mm-ss") == dateTime)
                {
                    var _data = new VoltageSagData.voltageSagData
                    {
                        treggerDateTime=item.treggerDateTime,
                        StartTime=item.StartTime,
                        duration=item.duration,
                        cycle=item.cycle,
                        PValue=item.PValue,
                        QValue=item.QValue,
                        SValue=item.SValue,
                        strPSValue=strPSvalue
                    };
                    NewData.Add(_data);
                }
                else
                    NewData.Add(item);
            }
            SaveXml(NewData.ToArray());
        }
        private string GetXmlString(string strNode)
        {
            //使用XmlDocument讀入XML格式資料
            XmlDocument xmlDoc = new XmlDocument();
            // string strPath = System.Windows.Forms.Application.StartupPath + strXmlFile;
            xmlDoc.Load(_filePaht);
            //使用XmlNode讀取節點
            XmlNode strTag = xmlDoc.SelectSingleNode(strNode); //注意節點的指定方式
            return strTag.InnerText;
        }
        public VoltageSagData.voltageSagData[] GetXmlData()
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(_filePaht);

            var reData = new List<VoltageSagData.voltageSagData>();

            foreach (XmlNode item in xmlDoc.SelectNodes("root/Data"))
            {
                try
                {
                    var _data = new VoltageSagData.voltageSagData()
                    {
                        treggerDateTime = Convert.ToDateTime(item.SelectSingleNode("TreggerDateTime").InnerText),
                        duration = Convert.ToDecimal(item.SelectSingleNode("Duration").InnerText),
                        StartTime = Convert.ToDecimal(item.SelectSingleNode("StartTime").InnerText),
                        cycle = Convert.ToDecimal(item.SelectSingleNode("Cycle").InnerText),
                        PValue = Convert.ToDecimal(item.SelectSingleNode("PerUnitValue/P").InnerText),
                        QValue = Convert.ToDecimal(item.SelectSingleNode("PerUnitValue/Q").InnerText),
                        SValue = Convert.ToDecimal(item.SelectSingleNode("PerUnitValue/S").InnerText),
                        strPSValue = item.SelectSingleNode("PSValue").InnerText.ToString()
                    };
                    reData.Add(_data);
                }
                catch (Exception ex) { }
            }
            return reData.ToArray();
        }
        public void SaveXml(VoltageSagData.voltageSagData[] datas)
        {
            string reString = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(_filePaht);

                xmlDoc.RemoveAll();

                XmlElement company = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(company);
                ////建立子節點
                //XmlElement department = xmlDoc.CreateElement("Data");

                ////加入至company節點底下
                //company.AppendChild(department);

                foreach (var item in datas)
                {
                    XmlElement _Data = xmlDoc.CreateElement("Data");

                    XmlElement _DateTime = xmlDoc.CreateElement("TreggerDateTime");
                    _DateTime.InnerText = item.treggerDateTime.ToString();
                    XmlElement _Duration = xmlDoc.CreateElement("Duration");
                    _Duration.InnerText = item.duration.ToString();
                    XmlElement _StartTime = xmlDoc.CreateElement("StartTime");
                    _StartTime.InnerText = item.StartTime.ToString();
                    XmlElement _Cycle = xmlDoc.CreateElement("Cycle");
                    _Cycle.InnerText = item.cycle.ToString();

                    XmlElement _Value = xmlDoc.CreateElement("PerUnitValue");

                    XmlElement _P = xmlDoc.CreateElement("P");
                    _P.InnerText = item.PValue.ToString();
                    XmlElement _Q = xmlDoc.CreateElement("Q");
                    _Q.InnerText = item.QValue.ToString();
                    XmlElement _S = xmlDoc.CreateElement("S");
                    _S.InnerText = item.SValue.ToString();

                    _Value.AppendChild(_P);
                    _Value.AppendChild(_Q);
                    _Value.AppendChild(_S);
                    //加入至company節點底下

                    XmlElement _PSValue = xmlDoc.CreateElement("PSValue");
                    _PSValue.InnerText = item.strPSValue;

                    _Data.AppendChild(_DateTime);
                    _Data.AppendChild(_Duration);
                    _Data.AppendChild(_StartTime);
                    _Data.AppendChild(_Cycle);
                    _Data.AppendChild(_Value);
                    _Data.AppendChild(_PSValue);

                    company.AppendChild(_Data);
                }
                xmlDoc.Save(_filePaht);

            }
            catch (Exception e) { throw; }
        }
    }
}
