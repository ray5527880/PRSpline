using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace PRSpline
{
    class FTPDownload
    {
        public string[] GetFTPFileName(string path, string user, string pwd)
        {
            List<string> strList = new List<string>();
            FtpWebRequest f = (FtpWebRequest)WebRequest.Create(new Uri(path + "records/"));
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
                        //str = str.Substring(str.IndexOf("/") + 1, str.Length - str.IndexOf("/"));
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
        public void FTP_Download(string fileName, string Path, string User, string Pwd)
        {
            //連接+指定檔案
            //ftp://++/ftp/
            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://" + Path + "/ftp/records/" + fileName);
            //登入
            requestFileDownload.Credentials = new NetworkCredential(User, Pwd);
            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
            try
            {
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                //下載
                FileStream writeStream = new FileStream(EditXml.strDownloadPath + fileName, FileMode.Create);

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
            catch { }



        }
        // 上傳從FTP(上傳檔案名稱)
        //public void FTP_Updata(string fileName)
        //{

        //    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(EditXml.strFTPHost + "/" + fileName);
        //    requestFTPUploader.Credentials = new NetworkCredential(EditXml.strFTPUser, EditXml.strFTPPwd);
        //    requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

        //    FileInfo fileInfo = new FileInfo(EditXml.strDownloadPath + fileName);
        //    FileStream fileStream = fileInfo.OpenRead();

        //    int bufferLength = 2048;
        //    byte[] buffer = new byte[bufferLength];

        //    Stream uploadStream = requestFTPUploader.GetRequestStream();
        //    int contentLength = fileStream.Read(buffer, 0, bufferLength);

        //    while (contentLength != 0)
        //    {
        //        uploadStream.Write(buffer, 0, contentLength);
        //        contentLength = fileStream.Read(buffer, 0, bufferLength);
        //    }

        //    uploadStream.Close();
        //    fileStream.Close();

        //    requestFTPUploader = null;
        //}
    }
}
