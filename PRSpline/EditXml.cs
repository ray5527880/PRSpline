using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PRSpline
{
    class EditXml
    {

        public class FTPData
        {
            public string strName;
            public string strIP;
            public string strUser;
            public string strPwd;
        }

        public static string strDownloadPath;
        public static string strXmlFile;
        public static int count;
        public static string[] strPR;

        public static List<FTPData> mFTPData;

        public  EditXml()
        {
            strXmlFile = this.GetType().Assembly.Location;
            strXmlFile = strXmlFile.Replace(".exe", ".xml");
            mFTPData = new List<FTPData>();
        }
        public void GetXmlData()
        {
            strDownloadPath = GetXmlString("root/downloadpath");

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(strXmlFile);

            foreach (XmlNode item in xmlDoc.SelectNodes("root/Data"))
            {
                var _FTPData = new FTPData()
                {
                    strName = item.SelectSingleNode("Name").InnerText,
                    strIP = item.SelectSingleNode("ftphost").InnerText,
                    strUser = item.SelectSingleNode("ftpuser").InnerText,
                    strPwd = item.SelectSingleNode("ftppwd").InnerText
                };
                mFTPData.Add(_FTPData);
            }
        }
        public static string SaveXml(string DownloadPath)
        {
            string reString = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(strXmlFile);

                xmlDoc.RemoveAll();

                XmlElement company = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(company);
                //建立子節點
                XmlElement department = xmlDoc.CreateElement("downloadpath");
                department.InnerText = DownloadPath;
                //加入至company節點底下
                company.AppendChild(department);

                foreach (var item in EditXml.mFTPData)
                {
                    XmlElement _Data = xmlDoc.CreateElement("Data");

                    XmlElement _Name = xmlDoc.CreateElement("Name");
                    _Name.InnerText = item.strName;

                    XmlElement _ftphost = xmlDoc.CreateElement("ftphost");
                    _ftphost.InnerText = item.strIP;
                    XmlElement _ftpuser = xmlDoc.CreateElement("ftpuser");
                    _ftpuser.InnerText = item.strUser;
                    XmlElement _ftppwd = xmlDoc.CreateElement("ftppwd");
                    _ftppwd.InnerText = item.strPwd;
                    //加入至company節點底下

                    _Data.AppendChild(_Name);
                    _Data.AppendChild(_ftphost);
                    _Data.AppendChild(_ftpuser);
                    _Data.AppendChild(_ftppwd);

                    company.AppendChild(_Data);
                }
                xmlDoc.Save(strXmlFile);

            }
            catch (Exception e) { reString = e.Message.ToString(); }
            return reString;
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
