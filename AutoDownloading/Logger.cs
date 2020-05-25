using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
namespace AutoDownloading
{
    class Logger
    {
        /// <summary>
        /// 取得或設定記錄的時間
        /// </summary>
        public string LogTime { get; set; }
        /// <summary>
        /// 取得或設定記錄的型別
        /// </summary>
        public string LogType { get; set; }
        /// <summary>
        /// 取得或設定需記錄的方法名稱
        /// </summary>
        public string FunctionName { get; set; }
        /// <summary>
        /// 取得或設定存放記錄檔的資料夾
        /// </summary>
        public string DirectoryPath { get; set; }
        /// <summary>
        /// 取得記錄檔的路徑
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// 取得記錄檔名
        /// </summary>
        public string FileName { get; private set; }

        
        public static void MakeLogger(string FunctionName, System.Data.SqlClient.SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append( ex.Errors[i].Message);
                errorMessages.Append("\x20");
            }
            MakeLoggerMessage(FunctionName, errorMessages.ToString());
        }
        public static void MakeLogger(string FunctionName,string Message)
        {
            MakeLoggerMessage(FunctionName, Message);
        }
        public static void MakeLogger(string FunctionName, InvalidOperationException ex)
        {
            MakeLoggerMessage(FunctionName, ex.Message.ToString());
        }
        public static void MakeLogger(string FunctionName, ArgumentException ex)
        {
            MakeLoggerMessage(FunctionName, ex.Message.ToString());
        }
        public static void MakeLogger(string FunctionName, InvalidCastException ex)
        {
            MakeLoggerMessage(FunctionName, ex.Message.ToString());
        }
        public static void MakeLogger(string FunctionName, Exception ex)
        {
            MakeLoggerMessage(FunctionName, ex.Message.ToString());
        }

        private static void MakeLoggerMessage(string FunctionName, params string[] contents)
        {
            Logger logger = new Logger();
            logger.LogTime = DateTime.Now.ToLongTimeString();
            logger.LogType = "ERROR";
            logger.FunctionName = FunctionName;
            logger.AppandMessage(contents);
            logger.CloseFile();
        }
        public Logger()
        {
            var filename = "datacollect_" + DateTime.Now.ToString("yyyyMMdd");
            var filepath = this.GetType().Assembly.Location;
            this.DirectoryPath = filepath.Replace("PRSpline.exe", @"\log");           
            filepath = DirectoryPath + @"\" + filename + ".txt";

            this.FilePath = filepath;
            this.FileName = filename;
        }

        public Logger CreateFile()
        {
            if (!File.Exists(this.FilePath))
            {
                if (!Directory.Exists(this.DirectoryPath))
                {
                    Directory.CreateDirectory(this.DirectoryPath);
                }

                using (File.CreateText(this.FilePath)) { }
            }

            return this;
        }
        public Logger AppandMessage(params string[] contents)
        {
            using (var writer = File.AppendText(this.FilePath))
            {
                writer.Write(this.LogTime);
                writer.Write("\x20");
                writer.Write(this.LogType);
                writer.Write("\x20");
                writer.Write("\x5B");
                writer.Write("\x28");
                writer.Write(this.FunctionName);
                writer.Write("\x29");
                writer.Write("\x20");

                var count = 0;

                foreach (var item in contents)
                {
                    count++;

                    if (count != contents.Length)
                    {
                        writer.Write("\x28");
                        writer.Write(item);
                        writer.Write("\x29");
                        writer.Write("\x20");

                        continue;
                    }

                    writer.Write("\x28");
                    writer.Write(item);
                    writer.Write("\x29");
                    writer.Write("\x5D");
                    writer.Write("\x20");
                }
            }

            return this;
        }

        public Logger CloseFile()
        {
            using (var writer = File.AppendText(this.FilePath))
            {
                writer.WriteLine();
            }

            return this;
        }
    }
}
