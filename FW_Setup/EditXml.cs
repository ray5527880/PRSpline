using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BF_FW
{
    public class EditXml
    {
        public static string MeterFilePaht;
        public static string RelayFilePaht;

        public static string DBPath;
        public static string DBName;
        public static string DBUser;
        public static string DBPwd;

        public static int IsUserSQL;

        public static int VoltageSag;
        public class FTPData
        {
            public string strName;
            public string strIP;
            public string strUser;
            public string strPwd;
            public int BaseValue;
        }

        public static string strDownloadPath;

        public static string strXmlFile;

        public static int m_nTimes;

        public static List<FTPData> mFTPData;

        public EditXml()
        {
            strXmlFile = this.GetType().Assembly.Location;
            strXmlFile = strXmlFile.Replace("FWAutoDownloading.exe", "PRSpline.xml");
            strXmlFile = strXmlFile.Replace("fwsetup.dll", "PRSpline.xml");
            strXmlFile = strXmlFile.Replace("PRSpline.exe", "PRSpline.xml");
            strDownloadPath = strXmlFile.Replace("PRSpline.xml", @"downloadFile\");
            mFTPData = new List<FTPData>();
        }
        public void GetVoltageSag()
        {
            VoltageSag = Convert.ToInt32(GetXmlString("root/VoltageSag"));
        }
        public void GetMorRFilePaht()
        {
            MeterFilePaht = GetXmlString("root/MeterFilePaht");
            RelayFilePaht = GetXmlString("root/RelayFilePaht");
        }
        public void GetXmlData()
        {
            m_nTimes = Convert.ToInt32(GetXmlString("root/period/min"));
            DBPath = GetXmlString("root/connectionAlarms/server");
            DBName = GetXmlString("root/connectionAlarms/database");
            DBUser = GetXmlString("root/connectionAlarms/uid");
            DBPwd = GetXmlString("root/connectionAlarms/pwd");
            IsUserSQL = Convert.ToInt32(GetXmlString("root/IsUserSQL"));
            GetVoltageSag();
            GetMorRFilePaht();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(strXmlFile);

            foreach (XmlNode item in xmlDoc.SelectNodes("root/Data"))
            {

                var _FTPData = new FTPData()
                {
                    strName = item.SelectSingleNode("Name").InnerText,
                    strIP = item.SelectSingleNode("ftphost").InnerText,
                    strUser = item.SelectSingleNode("ftpuser").InnerText,
                    strPwd = item.SelectSingleNode("ftppwd").InnerText,
                    //BaseValue=Convert.ToInt32(item.SelectSingleNode("BaseValue").InnerText)

                };
                var x = item.SelectSingleNode("BaseValue").InnerText;
                _FTPData.BaseValue = item.SelectSingleNode("BaseValue").InnerText != String.Empty ? Convert.ToInt32(item.SelectSingleNode("BaseValue").InnerText) : 0;
                mFTPData.Add(_FTPData);
            }
        }
        public static string SaveXml()
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
                XmlElement department = xmlDoc.CreateElement("period");
                XmlElement department1 = xmlDoc.CreateElement("min");

                department1.InnerText = m_nTimes.ToString();
                department.AppendChild(department1);

                //加入至company節點底下
                XmlElement UserSQL = xmlDoc.CreateElement("IsUserSQL");
                UserSQL.InnerText = IsUserSQL.ToString();

                company.AppendChild(department);
                company.AppendChild(UserSQL);

                XmlElement UserVS = xmlDoc.CreateElement("VoltageSag");
                UserVS.InnerText = VoltageSag.ToString();

                company.AppendChild(department);
                company.AppendChild(UserVS);

                XmlElement meterpath = xmlDoc.CreateElement("MeterFilePaht");
                meterpath.InnerText = MeterFilePaht;

                company.AppendChild(department);
                company.AppendChild(meterpath);

                XmlElement relayrpath = xmlDoc.CreateElement("RelayFilePaht");
                relayrpath.InnerText = RelayFilePaht;

                company.AppendChild(department);
                company.AppendChild(relayrpath);

                XmlElement connectionAlarms = xmlDoc.CreateElement("connectionAlarms");
                XmlElement server = xmlDoc.CreateElement("server");
                XmlElement database = xmlDoc.CreateElement("database");
                XmlElement uid = xmlDoc.CreateElement("uid");
                XmlElement pwd = xmlDoc.CreateElement("pwd");

                server.InnerText = DBPath;
                database.InnerText = DBName;
                uid.InnerText = DBUser;
                pwd.InnerText = DBPwd;


                connectionAlarms.AppendChild(server);
                connectionAlarms.AppendChild(database);
                connectionAlarms.AppendChild(uid);
                connectionAlarms.AppendChild(pwd);
                //加入至company節點底下
                company.AppendChild(connectionAlarms);

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
                    XmlElement _baseValue = xmlDoc.CreateElement("BaseValue");
                    _baseValue.InnerText = item.BaseValue.ToString();
                    //加入至company節點底下

                    _Data.AppendChild(_Name);
                    _Data.AppendChild(_ftphost);
                    _Data.AppendChild(_ftpuser);
                    _Data.AppendChild(_ftppwd);
                    _Data.AppendChild(_baseValue);

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
