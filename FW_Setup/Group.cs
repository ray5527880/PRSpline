using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BF_FW
{
    public class GroupData
    {
        public  int No;
        public string MainFileName;
        public string[] childFileName;
        public string Remarks;
    }
    public class Group
    {


        public static List<GroupData> GroupDatas;

        string strXmlFile;
        public Group()
        {
            strXmlFile = this.GetType().Assembly.Location;
            strXmlFile = strXmlFile.Replace("FWAutoDownloading.exe", "Group.xml");
            strXmlFile = strXmlFile.Replace("fwsetup.dll", "Group.xml");
            strXmlFile = strXmlFile.Replace("PRSpline.exe", "Group.xml");
            GroupDatas = new List<GroupData>();
            GetGroupData();
        }
        public void GetGroupData()
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(strXmlFile);
            int _No = 1;
            foreach (XmlNode item in xmlDoc.SelectNodes("root/Data"))
            {
                var _GroupData = new GroupData();
                _GroupData.No = _No;
                _GroupData.MainFileName = item.SelectSingleNode("MainFileName").InnerText;
                _GroupData.Remarks = item.SelectSingleNode("Remarks").InnerText;
                var _childlist = new List<string>();
                foreach(XmlNode items in item.SelectNodes("childFileName"))
                {
                    _childlist.Add(items.SelectSingleNode("FilesName").InnerText);
                }
                _GroupData.childFileName = _childlist.ToArray();
                _No++;
                GroupDatas.Add(_GroupData);
            }
        }
        public bool SaveXml()
        {
            bool reValue = false;
            string reString = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(strXmlFile);

                xmlDoc.RemoveAll();

                XmlElement company = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(company);
                //建立子節點
            
                foreach (var item in GroupDatas)
                {
                    XmlElement _Data = xmlDoc.CreateElement("Data");

                    XmlElement _MainFileName = xmlDoc.CreateElement("MainFileName");
                    _MainFileName.InnerText = item.MainFileName;

                    XmlElement _Remarks = xmlDoc.CreateElement("Remarks");
                    _Remarks.InnerText = item.Remarks;
                    XmlElement _childFileName = xmlDoc.CreateElement("childFileName");
                    foreach(var item_2 in item.childFileName)
                    {
                        XmlElement _FileName = xmlDoc.CreateElement("FilesName");
                        _FileName.InnerText = item_2;
                        _childFileName.AppendChild(_FileName);
                    }            
                    //加入至company節點底下

                    _Data.AppendChild(_MainFileName);
                    _Data.AppendChild(_Remarks);
                    _Data.AppendChild(_childFileName);

                    company.AppendChild(_Data);
                }
                xmlDoc.Save(strXmlFile);
                reValue = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return reValue;
        }
        private string GetXmlString(string strNode)
        {
            //使用XmlDocument讀入XML格式資料
            XmlDocument xmlDoc = new XmlDocument();
            // string strPath = System.Windows.Forms.Application.StartupPath + strXmlFile;
            xmlDoc.Load(strXmlFile);
            //使用XmlNode讀取節點
            XmlNode strTag = xmlDoc.SelectSingleNode(strNode); //注意節點的指定方式
            return strTag.InnerText;
        }


    }
}
