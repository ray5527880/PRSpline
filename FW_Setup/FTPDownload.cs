using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;

namespace BF_FW
{
    public class FTPDownload
    {
        public bool CheckConnection(string path, string user, string pwd)
        {
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + path + "/_LD/PROT/COMTRADE/"));
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(user, pwd);
                var response = request.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {                
                return false;
            }
            finally
            {
                request = null;
            }

            return true;
        }

        public string[] GetFTPFileName(string path, string user, string pwd)
        {
            List<string> strList = new List<string>();
            FtpWebRequest f = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + path + "/_LD/PROT/COMTRADE/"));
            f.Method = WebRequestMethods.Ftp.ListDirectory;
            f.UseBinary = true;
            f.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            f.Credentials = new NetworkCredential(user, pwd);

            using (StreamReader sReader = new StreamReader(f.GetResponse().GetResponseStream()))
            {
                string str = sReader.ReadLine();

                while (str != null)
                {
                    while (str.IndexOf("/") > 1)
                    {
                        int stringlength = str.Length;
                        int stringindexof=str.IndexOf("/");
                        
                        str = str.Substring((stringindexof + 1), (stringlength - stringindexof));
                    }
                    strList.Add(str);
                    str = sReader.ReadLine();
                }
            }
            string[] outstr = new string[strList.Count];
            int count = 0;
            foreach (var item in strList)
            {
                outstr[count] = item;
                count++;
            }
            return outstr;
        }
        // 下載從FTP(下載檔案名稱)
        public void FTP_Download(string filePath, string fileName, string Path, string User, string Pwd)
        {
            //連接+指定檔案
            //ftp://++/ftp/
            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://" + Path + "/_LD/PROT/COMTRADE/" + fileName);
            //登入
            requestFileDownload.Credentials = new NetworkCredential(User, Pwd);
            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
            try
            {
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                //下載
                FileStream writeStream = new FileStream(filePath, FileMode.Create);

                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                responseStream.Close();
                writeStream.Close();
                requestFileDownload = null;
                responseFileDownload = null;
            }
            catch (Exception ex)
            {
                string FunctionName = MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name;

                Logger.MakeLogger(FunctionName, ex);
            }
        }

        public void FTP_Delete(string fileName, string Path, string User, string Pwd)
        {
            //連接+指定檔案
            //ftp://++/ftp/
            FtpWebRequest requestFilDelete = (FtpWebRequest)WebRequest.Create("ftp://" + Path + "/ftp/records/" + fileName);
            //登入
            requestFilDelete.Credentials = new NetworkCredential(User, Pwd);
            requestFilDelete.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFilDelete.GetResponse();
            }
            catch (Exception ex)
            {
                string FunctionName = MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name;

                Logger.MakeLogger(FunctionName, ex);
            }
        }       
    }
}
