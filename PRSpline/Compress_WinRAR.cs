using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using Microsoft.Win32;

namespace PRSpline
{
    class Compress_WinRAR
    {
        private bool ExistsRar(ref String winRarPath)
        {
            //通过Regedit（注册表）找到WinRar文件
            var registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            if (registryKey == null) return false;//未安装
            //registryKey = theReg;可以直接返回Registry对象供会面操作
            winRarPath = registryKey.GetValue("").ToString();
            //这里为节约资源，直接返回路径，反正下面也没用到
            registryKey.Close();//关闭注册表
            return !String.IsNullOrEmpty(winRarPath);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="unRarPath">文件夹路径</param>
        /// <param name="rarPath">压缩文件的路径</param>
        /// <param name="rarName">压缩文件的文件名</param>
        /// <returns></returns>
        public String UnCompressRar(String unRarPath, String rarPath, String rarName)
        {
            try
            {
                String winRarPath = string.Empty;
                if (!ExistsRar(ref winRarPath)) return "";
                //验证WinRar是否安装。
                if (Directory.Exists(unRarPath) == false)
                {
                    Directory.CreateDirectory(unRarPath);
                }
                var pathInfo = "x " + rarName + " " + unRarPath + " -y";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = winRarPath,//执行的文件名
                        Arguments = pathInfo,//需要执行的命令
                        UseShellExecute = false,//使用Shell执行
                        WindowStyle = ProcessWindowStyle.Hidden,//隐藏窗体
                        WorkingDirectory = rarPath,//rar 存放位置
                        CreateNoWindow = false,//不显示窗体
                    },
                };
                process.Start();//开始执行
                process.WaitForExit();//等待完成并退出
                process.Close();//关闭调用 cmd 的什么什么
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return unRarPath;
        }

        public  void CompressRar(String path, String rarPath, String rarName)
        {
            try
            {
                String winRarPath = null;
                if (!ExistsRar(ref winRarPath)) return;
                //验证WinRar是否安装。
                var pathInfo = String.Format("a -afzip -m0 -ep1 \"{0}\" \"{1}\"", rarName, path);
                #region WinRar 用到的命令注释
                //[a] 添加到壓縮文件
                //afzip 執行zip壓縮方式，方便用戶在不同環境下使用。
                //（取消該參數則執行rar壓縮）
                //-m0 存儲 添加到壓縮文件時不壓縮文件。共6個級別【0-5】，值越大效果越好，也越慢
                //ep1 依名稱排除主目錄（生成的壓縮文件不會出現不必要的層級）
                //r 修復壓縮檔案
                //t 測試壓縮檔案內的文件
                //as 同步壓縮檔案內容
                //-p 給壓縮文件加密碼方式為：-p123456
                #endregion
                //打包文件存放目錄
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = winRarPath,//执行的文件名
                        Arguments = pathInfo,//需要执行的命令
                        UseShellExecute = false,//使用Shell执行
                        WindowStyle = ProcessWindowStyle.Hidden,//隐藏窗体
                        WorkingDirectory = rarPath,//rar 存放位置
                        CreateNoWindow = false,//不显示窗体
                    },
                };
                process.Start();//开始执行
                process.WaitForExit();//等待完成并退出
                process.Close();//关闭调用 cmd 的什么什么
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
